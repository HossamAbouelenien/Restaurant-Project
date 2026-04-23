using System.Security.Cryptography;
using System.Text;

namespace RMS.Services.PaymobServices;

public static class PaymobHmacHelper
{
    public static bool ValidateHmac(PaymobWebhookDto data, string secret)
    {
        var obj = data.Obj;

        var concatenated =
            obj.AmountCents +
            obj.CreatedAt +
            obj.Currency +
            obj.ErrorOccured +
            obj.HasParentTransaction +
            obj.Id +
            obj.IntegrationId +
            obj.Is3DSecure +
            obj.IsAuth +
            obj.IsCapture +
            obj.IsRefunded +
            obj.IsStandalonePayment +
            obj.IsVoided +
            obj.Order +
            obj.Owner +
            obj.Pending +
            obj.SourceDataPan +
            obj.SourceDataSubType +
            obj.SourceDataType +
            obj.Success;

        var computed = Compute(concatenated, secret);

        return string.Equals(computed, data.Hmac, StringComparison.OrdinalIgnoreCase);
    }

    private static string Compute(string data, string secret)
    {
        var key = Encoding.UTF8.GetBytes(secret);
        var bytes = Encoding.UTF8.GetBytes(data);

        using var hmac = new HMACSHA512(key);
        var hash = hmac.ComputeHash(bytes);

        return Convert.ToHexString(hash).ToLower();
    }
}