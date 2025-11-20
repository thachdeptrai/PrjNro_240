namespace Tab2{using UnityEngine.Networking;

public class BypassCertificateHandler : CertificateHandler
{
	protected override bool ValidateCertificate(byte[] certificateData)
	{
		return true;
	}
}
}