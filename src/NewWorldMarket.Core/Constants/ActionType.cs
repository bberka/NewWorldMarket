namespace NewWorldMarket.Core.Constants;

public enum ActionType
{
    None,
    AccountLogin = 100,
    AccountLogout,
    AccountRegister,
    AccountGetOrders,
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
    AccountSettings,

    OrderCreate = 300,
    OrderUpdate,
    OrderCancel,
    OrderExpire,
    OrderReport,
    OrderGet,
    OrderComplete,
    OrderActivate,
    
    EmailSend = 400,
    EmailSendEmailVerification,
    EmailSendPasswordReset,
    EmailSendPasswordChanged,
    EmailSendOrderCancel,

    ImageGet = 500,
    ImageUpload,
    ImageDelete,
    ImageUpdate,
    






}