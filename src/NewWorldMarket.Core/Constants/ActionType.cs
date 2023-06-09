namespace NewWorldMarket.Core.Constants;

public enum ActionType
{
    None,
    Login = 100,
    Logout,
    Register,


    AccountGetOrders = 200,
    AccountGetCharacters,
    AccountVerifyEmail,
    AccountChangePassword,
    AccountChangeEmail,
    AccountChangeUsername,
    AccountChangeAvatar,
    AccountAddCharacter,
    AccountRemoveCharacter,
    AccountLinkDiscord,
    AccountLinkSteam,
    AccountUnlinkDiscord,
    AccountUnlinkSteam,
    AccountGet,

    BuyOrderCreate = 300,
    BuyOrderUpdate,
    BuyOrderCancel,
    BuyOrderExpire,
    BuyOrderReport,
    BuyOrderGet,


    SellOrderCreate = 400,
    SellOrderUpdate,
    SellOrderCancel,
    SellOrderExpire,
    SellOrderReport,
    SellOrderGet,

    EmailSend = 500,
    EmailSendEmailVerification,
    EmailSendPasswordReset,
    EmailSendPasswordChanged,
    EmailSendOrderCancel,








}