using EasMe;
using EasMe.Extensions;
using EasMe.Result;
using Google.Api.Gax.ResourceNames;
using Google.Cloud.RecaptchaEnterprise.V1;
using Microsoft.AspNetCore.Http;
using NewWorldMarket.Core.Constants;
using static System.Console;

namespace NewWorldMarket.Core.Tools;

#if CAPTHCA_ENABLED
public class CaptchaMgr
{

    private CaptchaMgr()
    {
        if (!ConstMgr.IsDevelopment)
        {
            var keyFromConfig = EasConfig.GetString("CaptchaKey");
            if (keyFromConfig.IsNullOrEmpty())
                throw new Exception("Captcha Key can not be empty");
            var secretFromConfig = EasConfig.GetString("CaptchaSecret");
            if (secretFromConfig.IsNullOrEmpty())
                throw new Exception("Captcha Secret can not be empty");
            Key = keyFromConfig;
            Secret = secretFromConfig;
        }

        var name = EasConfig.GetString("CaptchaProjectName");
        _client = RecaptchaEnterpriseServiceClient.Create();
        _projectName = new ProjectName(name);
    }
	public static CaptchaMgr This
	{
		get
		{
			Instance ??= new();
			return Instance;
		}
	}
	private static CaptchaMgr? Instance;
	private static RecaptchaEnterpriseServiceClient? _client;
	private static ProjectName? _projectName;
    private string Key = string.Empty;
    private string Secret = string.Empty;

    public string GetKey() => Key;
    public Result Validate(HttpContext context)
    {
        if (ConstMgr.IsDevelopment) 
            return Result.Success();
        var captchaResponse = context.Request.Form["g-recaptcha-response"];
        var captcha = EasReCaptcha.Validate(Key, captchaResponse);
        if (!captcha.Success)
        {
            return Result.Error("You must verify Google ReCaptcha");
        }
        return Result.Success();
    }

    //public void createAssessment(
    //   string token = "action-token", string recaptchaAction = "action-name")
    //{

    //    // Create the client.
    //    // TODO: To avoid memory issues, move this client generation outside
    //    // of this example, and cache it (recommended) or call client.close()
    //    // before exiting this method.


    //    // Build the assessment request.
    //    var createAssessmentRequest = new CreateAssessmentRequest()
    //    {
    //        Assessment = new Assessment()
    //        {
    //            // Set the properties of the event to be tracked.
    //            Event = new Event()
    //            {
    //                SiteKey = Key,
    //                Token = token,
    //                ExpectedAction = recaptchaAction
    //            },
    //        },
    //        ParentAsProjectName = projectName
    //    };

    //    var response = _client.CreateAssessment(createAssessmentRequest);

    //    // Check if the token is valid.
    //    if (response.TokenProperties.Valid == false)
    //    {
    //        WriteLine("The CreateAssessment call failed because the token was: " +
    //            response.TokenProperties.InvalidReason.ToString());
    //        return;
    //    }

    //    // Check if the expected action was executed.
    //    if (response.TokenProperties.Action != recaptchaAction)
    //    {
    //        WriteLine("The action attribute in reCAPTCHA tag is: " +
    //            response.TokenProperties.Action.ToString());
    //        WriteLine("The action attribute in the reCAPTCHA tag does not " +
    //            "match the action you are expecting to score");
    //        return;
    //    }

    //    // Get the risk score and the reason(s).
    //    // For more information on interpreting the assessment,
    //    // see: https://cloud.google.com/recaptcha-enterprise/docs/interpret-assessment
    //    WriteLine("The reCAPTCHA score is: " + ((decimal)response.RiskAnalysis.Score));

    //    foreach (var reason in response.RiskAnalysis.Reasons)
    //    {
    //        WriteLine(reason.ToString());
    //    }
    //}

}

#endif