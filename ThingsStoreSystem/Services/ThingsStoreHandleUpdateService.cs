namespace ThingsStore.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CSharpFunctionalExtensions;

    using global::ThingsStore.Constants;
    using global::ThingsStore.Pagination;

    using Lagalike.Telegram.Shared;
    using Lagalike.Telegram.Shared.Contracts;
    using Lagalike.Telegram.Shared.Services;

    using Newtonsoft.Json;

    using Telegram.Bot.Types;
    using Telegram.Bot.Types.Enums;
    using Telegram.Bot.Types.ReplyMarkups;

    public class ThingsStoreHandleUpdateService : ITelegramUpdateHandler
    {
        private static readonly Pagination DefaultPagination = new()
        {
            Limit = 5,
            Offset = 0
        };

        private readonly IBank _bank;

        private readonly ConfiguredTelegramBotClient _botClient;

        private readonly InlineKeyboardMarkup _menuInlineKeyboard;

        private readonly ModeInfo _modeInfo;

        private readonly IDictionary<string, Func<CallbackQuery, Task>> _proccessCommands;

        private readonly PriceProducts _products;

        private readonly IThingsStore _thingsStore;

        private readonly string _titleMode;

        public ThingsStoreHandleUpdateService(ConfiguredTelegramBotClient botClient,
            ThingsStoreModeInfo modeInfo, IThingsStore thingsStore, PriceProducts products, IBank bank)
        {
            _botClient = botClient;
            _thingsStore = thingsStore;
            _products = products;
            _bank = bank;
            _titleMode = $"It's things store - {modeInfo.ModeInfo.ShortDescription}";
            _modeInfo = modeInfo.ModeInfo;

            var goShoppingButton = GetInlineKeyboardButton(AvailableDemoCommands.GoShopping);
            var showOrderHistory = GetInlineKeyboardButton(AvailableDemoCommands.ShowOrderHistory);
            var aboutModeButton = GetInlineKeyboardButton(AvailableDemoCommands.About);
            _menuInlineKeyboard = new InlineKeyboardMarkup(
                new[]
                {
                    new[] { goShoppingButton },
                    new[] { showOrderHistory },
                    new[] { aboutModeButton }
                }
            );
            _proccessCommands = new Dictionary<string, Func<CallbackQuery, Task>>
            {
                { AvailableDemoCommands.DemoMainMenu.CommandValue, ProccessMenuButtonsAsync },
                { AvailableDemoCommands.GoShopping.CommandValue, ProccessShoppingAsync },
                { AvailableDemoCommands.SelectItem.CommandValue, ProccessSelectItemAsync },
                { AvailableDemoCommands.BuyItem.CommandValue, ProccessBuyItemAsync },
                { AvailableDemoCommands.ShowOrderHistory.CommandValue, ProccessShowOrderHistoryAsync },
                { AvailableDemoCommands.ShowOrder.CommandValue, ProcessShowCurrentOrder },
            };
        }

        public async Task HandleUpdateAsync(Update update)
        {
            var action = update.Type switch
            {
                UpdateType.Message or UpdateType.EditedMessage => ProccessReceivedMessageAsync(update.Message),
                UpdateType.CallbackQuery => ProccessInlineKeyboardCallbackDataAsync(update.CallbackQuery),
                _ => Task.CompletedTask
            };

            await action;
        }

        private static Order BuildOrder(OrderInfo currentOrder, long customerId)
        {
            var orderInfos = currentOrder.OrderProducts.Values.Cast<OrderInformation>().ToList();
            var buyer = new BuyerInformation
            {
                BuyerId = customerId
            };

            return new Order
            {
                Buyer = buyer,
                OrderInfos = orderInfos
            };
        }

        private async Task EditMessageToNoOrderInfoMessage(CallbackQuery callbackQuery)
        {
            var staffKeyboard = new InlineKeyboardMarkup(GetEmptyOrderStaffBtns());
            await _botClient.EditMessageTextAsync(
                callbackQuery.From.Id,
                callbackQuery.Message.MessageId,
                "You have no any order 🔎🛒",
                replyMarkup: staffKeyboard
            );
        }

        private async Task EditMessageToOrderInfoMessage(CallbackQuery callbackQuery, OrderInfo order)
        {
            var products = order.OrderProducts.Values.Select(
                GetRemoveProductFromOrder);
            var customerId = callbackQuery.From.Id;
            var staffBtns = GetOrderStaffBtns(customerId);
            var items = products.Append(staffBtns).ToArray();
            var menuKeyboard = new InlineKeyboardMarkup(items);
            var productsPriceLabel = $"*The order price:* {order.OrderPrice}";
            var customer = GetPlayerInfo(customerId);
            var customerInfo = $"*Your money:* {customer.Balance}";
            var msg = $"Products in your basket 🛒.\n{customerInfo}\n{productsPriceLabel}";

            await _botClient.EditMessageTextAsync(
                callbackQuery.From.Id,
                callbackQuery.Message.MessageId,
                msg,
                ParseMode.Markdown,
                replyMarkup: menuKeyboard);
        }

        private static string FormatBuyInProductCard(Product product)
        {
            var productInfo = new ProductInfoToBuy
            {
                ProductId = product.Id,
                Menu = MenuWhereBoughtProduct.ProductCard,
                Type = CommandType.ProductInfoToBuy
            };

            return JsonConvert.SerializeObject(productInfo);
        }

        private static string FormatBuyInProductsList(Product product)
        {
            var productInfo = new ProductInfoToBuy
            {
                ProductId = product.Id,
                Menu = MenuWhereBoughtProduct.ProductsList,
                Type = CommandType.ProductInfoToBuy
            };

            return JsonConvert.SerializeObject(productInfo);
        }

        private static string FormatPagination(Pagination paging)
        {
            return JsonConvert.SerializeObject(paging);
        }

        private static string FormatPayForOrder(long customerId)
        {
            var dto = new PayForOrder
            {
                CustomerId = customerId,
                Type = CommandType.PayForOrder
            };

            return JsonConvert.SerializeObject(dto);
        }

        private static string FormatRemoveProductFromOrder(Product product)
        {
            var productToRemove = new RemoveProductFromOrder
            {
                ProductId = product.Id,
                Type = CommandType.RemoveProductFromOrder
            };

            return JsonConvert.SerializeObject(productToRemove);
        }

        private static string FormatSelectionItem(Product product)
        {
            var selectedProduct = new SelectedProduct
            {
                ProductId = product.Id,
                Type = CommandType.SelectedProduct
            };

            return JsonConvert.SerializeObject(selectedProduct);
        }

        private static string FormatViewOrderHistoryItem(OrderHistoryItemView data)
        {
            return JsonConvert.SerializeObject(data);
        }

        private static InlineKeyboardButton[] GetEmptyOrderStaffBtns()
        {
            var backToShoppingBtn = GetInlineKeyboardButton(AvailableDemoCommands.BackToShopping);

            return new[] { backToShoppingBtn };
        }

        private static InlineKeyboardButton GetInlineKeyboardButton(CommandInfo command)
        {
            var (userLabel, commandValue) = command;
            return InlineKeyboardButton.WithCallbackData(userLabel, commandValue);
        }

        private static InlineKeyboardButton[] GetItem(Product product)
        {
            var productBtn = InlineKeyboardButton.WithCallbackData(
                product.Label,
                FormatSelectionItem(product));
            var addBtn = InlineKeyboardButton.WithCallbackData($"💵 {product.Cost}", FormatBuyInProductsList(product));

            return new[]
            {
                productBtn, addBtn
            };
        }

        private static InlineKeyboardButton[][] GetMenuProduct(Product product)
        {
            var addBtn = new[]
            {
                InlineKeyboardButton.WithCallbackData($"💵 {product.Cost}", FormatBuyInProductCard(product))
            };
            var backToListProductsBtn = new[]
            {
                GetInlineKeyboardButton(AvailableDemoCommands.BackToShopping)
            };

            return new[]
            {
                addBtn, backToListProductsBtn
            };
        }

        private static OrderHistoryItemView GetOrderHistoryItem(string data)
        {
            return JsonConvert.DeserializeObject<OrderHistoryItemView>(data);
        }

        private static InlineKeyboardMarkup GetOrdersHistoryKeyboard(Paged<OrderOverView> historyOfOrders, Pagination paging)
        {
            
            var items = historyOfOrders.Items.Select(
                                           (x, i) =>
                                           {
                                               var label =
                                                   $"👀Order №{i + 1}, 💰 {x.SpentMoney}, 📦 {x.BoughtItemsAmount}";
                                               var viewOrderInfoEvent = new OrderHistoryItemView
                                               {
                                                   OrderId = x.OrderId,
                                                   Type = CommandType.OrderHistoryItemView,
                                                   Paging = DefaultPagination
                                               };
                                               var callbackData = FormatViewOrderHistoryItem(viewOrderInfoEvent);
                                               var btn = InlineKeyboardButton.WithCallbackData(label, callbackData);

                                               return new[] { btn };
                                           })
                                       .ToArray();

            var pagingBtns = GetPagingBtns(paging);
            var staffBtns = new[] { new[] { GetInlineKeyboardButton(AvailableDemoCommands.DemoMainMenu) } };
            var btns = items.Concat(pagingBtns).Concat(staffBtns).ToArray();
            var menuKeyboard = new InlineKeyboardMarkup(btns);

            return menuKeyboard;
        }

        private static InlineKeyboardButton[] GetOrderStaffBtns(long customerId)
        {
            var payForOrderBtn = InlineKeyboardButton.WithCallbackData("💰 Pay for the order", FormatPayForOrder(customerId));
            var backToShoppingBtn = GetInlineKeyboardButton(AvailableDemoCommands.BackToShopping);

            return new[] { payForOrderBtn, backToShoppingBtn };
        }
        
        private static InlineKeyboardButton[][] GetOrderHistoryStaffBtns(Pagination paging, long itemsTotal)
        {
            var pagingBtns = GetPagingBtns(paging);
            var backToOrdersHistory = new [] { GetInlineKeyboardButton(AvailableDemoCommands.BackToOrdersHistory) };
            
            var isNeedToShowPagingBtns = itemsTotal > DefaultPagination.Limit;
            InlineKeyboardButton[][] btns;
            if (isNeedToShowPagingBtns)
            {
                btns = pagingBtns.Append(backToOrdersHistory).ToArray();  
            }
            else
            {
                btns = new [] { backToOrdersHistory };
            }

            return btns;
        }

        private static InlineKeyboardButton[][] GetPagingBtns(Pagination paging)
        {
            var startPage = DefaultPagination;
            var toStartPageBtn = InlineKeyboardButton.WithCallbackData(AvailableDemoCommands.GetStartItems.UserLabel, FormatPagination(startPage));
            var previousPage = paging with
            {
              Offset = paging.Offset - 1
            };//TODO: need to check the border conditions
            var previousPageBtn = InlineKeyboardButton.WithCallbackData(AvailableDemoCommands.GetPreviousItems.UserLabel, FormatPagination(previousPage));
            var nextPage = paging with
            {
                Offset = paging.Offset + 1
            }; //TODO: need to check the border conditions
            var nextPageBtn = InlineKeyboardButton.WithCallbackData(AvailableDemoCommands.GetNextItems.UserLabel, FormatPagination(nextPage));
            var endPage = paging;//TODO: need to calculate the last page
            var toEndPageBtn = InlineKeyboardButton.WithCallbackData(AvailableDemoCommands.GetEndItems.UserLabel, FormatPagination(endPage));

            return new[]
            {
                new[]
                {
                    toStartPageBtn,
                    previousPageBtn, 
                    nextPageBtn,
                    toEndPageBtn
                }
            };
        }

        private static Pagination GetPagingInfo(string data)
        {
            var isEmptyOrSystemCmd = string.IsNullOrEmpty(data) || data.Contains(ThingsStoreModeInfo.MODE_NAME);

            return isEmptyOrSystemCmd
                ? DefaultPagination
                : JsonConvert.DeserializeObject<Pagination>(data);
        }

        private Customer GetPlayerInfo(long customerId)
        {
            if (_bank.TryGetValue(customerId, out var customer))
                return customer;

            var newCustomer = new Customer
            {
                Balance = 400
            };
            _bank.Add(customerId, newCustomer);

            return newCustomer;
        }

        private Product GetProduct(string data)
        {
            var selectedProduct = JsonConvert.DeserializeObject<SelectedProduct>(data);
            var foundProduct = _products.Products.First(x => x.Id == selectedProduct.ProductId);

            return foundProduct;
        }

        private static ProductInfoToBuy GetProductInfo(string data)
        {
            return JsonConvert.DeserializeObject<ProductInfoToBuy>(data);
        }

        private static RemoveProductFromOrder GetProductToRemoveFromOrder(string data)
        {
            return JsonConvert.DeserializeObject<RemoveProductFromOrder>(data);
        }

        private static InlineKeyboardButton[] GetRemoveProductFromOrder(ProductInfo product)
        {
            var btnLabel = $"🗑{product.Product.Label}";

            return new[] { InlineKeyboardButton.WithCallbackData(btnLabel, FormatRemoveProductFromOrder(product.Product)) };
        }

        private static IEnumerable<InlineKeyboardButton[]> GetStaffBtns()
        {
            var orderBtn = GetInlineKeyboardButton(AvailableDemoCommands.ShowOrder);
            var backToThingsStoreBtn = GetInlineKeyboardButton(AvailableDemoCommands.DemoMainMenu);

            return new[]
            {
                new[]
                {
                    orderBtn, backToThingsStoreBtn
                }
            };
        }

        private static bool IsBuyProduct(string data)
        {
            return JsonConvert.DeserializeObject<ProductInfoToBuy>(data).Type == CommandType.ProductInfoToBuy;
        }

        private static bool IsPagingOrderHistory(string data)
        {
            return JsonConvert.DeserializeObject<PagingOrderHistory>(data)?.Type == CommandType.PagingOrderHistory;
        }

        private static bool IsPayForOrder(string data)
        {
            return JsonConvert.DeserializeObject<PayForOrder>(data)?.Type == CommandType.PayForOrder;
        }

        private static bool IsRemoveProductFromOrder(string data)
        {
            return JsonConvert.DeserializeObject<RemoveProductFromOrder>(data).Type == CommandType.RemoveProductFromOrder;
        }

        private static bool IsSelectProduct(string data)
        {
            return JsonConvert.DeserializeObject<SelectedProduct>(data)?.Type == CommandType.SelectedProduct;
        }

        private static bool IsViewOrderHistoryItem(string data)
        {
            return JsonConvert.DeserializeObject<OrderHistoryItemView>(data)?.Type == CommandType.OrderHistoryItemView;
        }

        private async Task ProccessBuyItemAsync(CallbackQuery callbackQuery)
        {
            var customerId = callbackQuery.From.Id;
            var productInfo = GetProductInfo(callbackQuery.Data);

            _thingsStore.AddProductToOrder(customerId, productInfo.ProductId);

            await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Added the product to your basket");

            await UpdateMenuInfoAsync(productInfo, callbackQuery);
        }

        private async Task ProccessInlineKeyboardCallbackDataAsync(CallbackQuery callbackQuery)
        {
            if (_proccessCommands.TryGetValue(callbackQuery.Data, out var proccessCommandFunc))
            {
                await proccessCommandFunc(callbackQuery);
            }
            else if (IsSelectProduct(callbackQuery.Data))
            {
                await ProccessSelectItemAsync(callbackQuery);
            }
            else if (IsBuyProduct(callbackQuery.Data))
            {
                await ProccessBuyItemAsync(callbackQuery);
            }
            else if (IsRemoveProductFromOrder(callbackQuery.Data))
            {
                await ProccessRemoveProductFromOrder(callbackQuery);
            }
            else if (IsPayForOrder(callbackQuery.Data))
            {
                await ProccessPayForOrder(callbackQuery);
            }
            else if (IsViewOrderHistoryItem(callbackQuery.Data))
            {
                await ProcessShowOrderHistoryItem(callbackQuery);
            }
            else if (IsProccessShopping(callbackQuery.Data))
            {
                await ProccessShoppingAsync(callbackQuery);
            }
            else
            {
                var msg = "It's nothing.";
                if (callbackQuery.Data == AvailableDemoCommands.About.CommandValue)
                    msg = _modeInfo.AboutDescription;

                await _botClient.SendTextMessageAsync(
                    callbackQuery.Message.Chat.Id,
                    msg);
            }
        }

        private static bool IsProccessShopping(string data)
        {
            return JsonConvert.DeserializeObject<ProductsMenuPagination>(data)?.Type == CommandType.ProductsMenu;
        }

        private async Task ProccessMenuButtonsAsync(CallbackQuery callbackQuery)
        {
            await _botClient.SendChatActionAsync(callbackQuery.Message.Chat.Id, ChatAction.Typing);

            await _botClient.EditMessageTextAsync(
                callbackQuery.Message.Chat.Id,
                callbackQuery.Message.MessageId,
                _titleMode,
                replyMarkup: _menuInlineKeyboard);
        }

        private async Task ProccessPayForOrder(CallbackQuery callbackQuery)
        {
            await _botClient.SendChatActionAsync(callbackQuery.Message.Chat.Id, ChatAction.Typing);

            var customerId = callbackQuery.From.Id;
            var currentOrder = _thingsStore.GetOrderInfo(customerId);
            if (currentOrder.HasNoValue)
            {
                await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "You have no any order to pay");
                return;
            }

            var order = BuildOrder(currentOrder.Value, customerId);
            var transactionResult = _thingsStore.Buy(order);

            if (transactionResult.IsSuccess)
            {
                await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Order has been paid");
                await ProcessShowCurrentOrder(callbackQuery);
            }
            else
            {
                await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id, transactionResult.Error.ToString());
            }
        }

        private async Task ProccessReceivedMessageAsync(Message message)
        {
            if (message.Type == MessageType.Text)
                await SendMenuButtonsAsync(message);
        }

        private async Task ProccessRemoveProductFromOrder(CallbackQuery callbackQuery)
        {
            var customerId = callbackQuery.From.Id;
            var productInfo = GetProductToRemoveFromOrder(callbackQuery.Data);

            _thingsStore.RemoveProductFromOrder(customerId, productInfo.ProductId);

            await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Removed the product to your basket");

            await ProcessShowCurrentOrder(callbackQuery);
        }

        private async Task ProccessSelectItemAsync(CallbackQuery callbackQuery)
        {
            var product = GetProduct(callbackQuery.Data);
            var customerId = callbackQuery.From.Id;
            var order = _thingsStore.GetOrderInfo(customerId)
                                    .Unwrap(
                                        new OrderInfo
                                        {
                                            OrderProducts = new Dictionary<long, ProductInfo>()
                                        });

            var customer = GetPlayerInfo(customerId);
            var orderInfo = $"💵 *Your money:* {customer.Balance}\n" +
                            $"🧺 *Products in basket:* {order.OrderProducts.Count}\n\n" +
                            $"🧺 *Total price of products:* {order.OrderPrice}";
            var msg = $"{orderInfo}\n\n{product.Description}";
            var items = GetMenuProduct(product);
            var itemsKeyboard = new InlineKeyboardMarkup(items);

            await _botClient.EditMessageTextAsync(
                callbackQuery.From.Id,
                callbackQuery.Message.MessageId,
                msg,
                ParseMode.Markdown,
                replyMarkup: itemsKeyboard);
        }

        private async Task ProccessShoppingAsync(CallbackQuery callbackQuery)
        {
            var paging = GetPagingInfo(callbackQuery.Data);
            var products = _thingsStore.GetItems(paging).Items.Select(GetItem);
            var navigationBtns = GetProductsPagingBtns(paging);
            var staffBtns = GetStaffBtns();
            var items = products.Concat(navigationBtns).Concat(staffBtns).ToArray();
            var itemsKeyboard = new InlineKeyboardMarkup(items);

            var customerId = callbackQuery.From.Id;
            var customer = GetPlayerInfo(customerId);
            var msg = $"Available items in the things store.\n💵 *Your money:* {customer.Balance}.";

            await _botClient.EditMessageTextAsync(
                callbackQuery.From.Id,
                callbackQuery.Message.MessageId,
                msg,
                ParseMode.Markdown,
                replyMarkup: itemsKeyboard);
        }

        private static InlineKeyboardButton[][] GetProductsPagingBtns(Pagination paging)
        {
            var productsPaging = new ProductsMenuPagination
            {
                Limit = paging.Limit,
                Offset = paging.Offset,
                Type = CommandType.ProductsMenu
            };

            return GetPagingBtns(productsPaging);
        }

        private async Task ProccessShowOrderHistoryAsync(CallbackQuery callbackQuery)
        {
            var customerId = callbackQuery.From.Id;
            var paging = GetPagingInfo(callbackQuery.Data);
            var historyOfOrders = _thingsStore.GetOrdersHistory(paging, customerId);
            if (!historyOfOrders.Items.Any())
            {
                const string EmptyOrderHistoryMsg = "💈 *Your have no any orders history* 👁";
                var staffBtns = new[] { new[] { GetInlineKeyboardButton(AvailableDemoCommands.DemoMainMenu) } };
                var menuKeyboard = new InlineKeyboardMarkup(staffBtns);
                await _botClient.EditMessageTextAsync(
                    callbackQuery.From.Id,
                    callbackQuery.Message.MessageId,
                    EmptyOrderHistoryMsg,
                    ParseMode.Markdown,
                    replyMarkup: menuKeyboard
                );
            }
            else
            {
                const string Msg = "💈 *Your orders history:*";
                var ordersKeyboard = GetOrdersHistoryKeyboard(historyOfOrders, paging);

                await _botClient.EditMessageTextAsync(
                    callbackQuery.From.Id,
                    callbackQuery.Message.MessageId,
                    Msg,
                    ParseMode.Markdown,
                    replyMarkup: ordersKeyboard);
            }
        }

        private async Task ProcessShowCurrentOrder(CallbackQuery callbackQuery)
        {
            await _botClient.SendChatActionAsync(callbackQuery.Message.Chat.Id, ChatAction.Typing);

            var order = _thingsStore.GetOrderInfo(callbackQuery.From.Id);
            if (order.HasValue && order.Value.OrderProducts.Keys.Any())
                await EditMessageToOrderInfoMessage(callbackQuery, order.Value);
            else
                await EditMessageToNoOrderInfoMessage(callbackQuery);
        }

        private async Task ProcessShowOrderHistoryItem(CallbackQuery callbackQuery)
        {
            await _botClient.SendChatActionAsync(callbackQuery.Message.Chat.Id, ChatAction.Typing);

            var customerId = callbackQuery.From.Id;
            var info = GetOrderHistoryItem(callbackQuery.Data);
            var orderProducts = _thingsStore.GetOrderHistory(info.Paging, customerId, info.OrderId);
            
            var staffBtns = GetOrderHistoryStaffBtns(info.Paging, orderProducts.Total);
            var orderProductsBtns = orderProducts.Items.Select(GetMenuOrderProduct);
            var items = orderProductsBtns.Concat(staffBtns).ToArray();
            var menuKeyboard = new InlineKeyboardMarkup(items);
            var orderPrice = 200;
            var productsPriceLabel = $"*The order price:* {orderPrice}";
            var productsAmount = $"Products amount: {orderProducts.Total}";
            var customer = GetPlayerInfo(customerId);
            var customerInfo = $"*Spent money:* {customer.Balance}";
            var msg = $"Order products menu.\n{customerInfo}\n{productsPriceLabel}\n{productsAmount}";

            await _botClient.EditMessageTextAsync(
                callbackQuery.From.Id,
                callbackQuery.Message.MessageId,
                msg,
                ParseMode.Markdown,
                replyMarkup: menuKeyboard);
        }

        private static InlineKeyboardButton[] GetMenuOrderProduct(OrderInformation orderInfo)
        {
            var itemInfo = new[]
            {
                InlineKeyboardButton.WithCallbackData(orderInfo.Product.Label)
            };

            return itemInfo;
        }

        private async Task SendMenuButtonsAsync(Message message)
        {
            await _botClient.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);

            await _botClient.SendTextMessageAsync(
                message.Chat.Id,
                _titleMode,
                replyMarkup: _menuInlineKeyboard);
        }

        private async Task UpdateMenuInfoAsync(ProductInfoToBuy info, CallbackQuery callbackQuery)
        {
            switch (info.Menu)
            {
                case MenuWhereBoughtProduct.ProductsList:
                    return;
                case MenuWhereBoughtProduct.ProductCard:
                    await ProccessSelectItemAsync(callbackQuery);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        $"Unknown type of a menu where bought the product: {callbackQuery.Data}");
            }
        }
    }

    internal record PagingOrderHistory
    {
        public CommandType Type { get; init; }
    }

    internal record ProductsMenuPagination : Pagination
    {
        public CommandType Type { get; init; }
    }

    internal record PayForOrder
    {
        public long CustomerId { get; init; }

        public CommandType Type { get; init; }
    }

    internal record OrderHistoryItemView
    {
        public long OrderId { get; init; }

        public Pagination Paging { get; init; } = null!;

        public CommandType Type { get; init; }
    }

    internal record SelectedProduct
    {
        public long ProductId { get; init; }

        public CommandType Type { get; init; }
    }

    internal record ProductInfoToBuy
    {
        public MenuWhereBoughtProduct Menu { get; init; }

        public long ProductId { get; init; }

        public CommandType Type { get; init; }
    }

    internal record RemoveProductFromOrder
    {
        public long ProductId { get; init; }

        public CommandType Type { get; init; }
    }

    internal enum CommandType
    {
        SelectedProduct,

        ProductInfoToBuy,

        RemoveProductFromOrder,

        PayForOrder,

        PagingOrderHistory,

        OrderHistoryItemView,
        
        ProductsMenu,
    }

    internal enum MenuWhereBoughtProduct
    {
        ProductsList,

        ProductCard
    }
}