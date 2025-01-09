public class SkillOption
{
	public int param;

	public SkillOptionTemplate optionTemplate;

	public string optionString;

	public string getOptionString()
	{
		if (optionString == null)
		{
			optionString = NinjaUtil.Replace(optionTemplate.name, "#", string.Empty + param);
		}
		return optionString;
	}
}
