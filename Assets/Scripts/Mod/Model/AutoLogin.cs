using System.Collections.Generic;

public class AutoLogin
{
    public bool waitToNextLogin = false;

    public long lastTimeWait = 0;

    public bool hasSetUserPass = false;

    public string accAutoLogin = "";

    public AutoLogin()
    {
        lastTimeWait = mSystem.currentTimeMillis();
    }

    public Account GetAccWithUsername(List<Account> accounts)
    {
        foreach (Account acc in accounts)
        {
            if (acc.getUsername().Equals(accAutoLogin))
            {
                return acc;
            }
        }

        return new Account("", "");
    }
}