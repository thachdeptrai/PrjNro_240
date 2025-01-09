public struct Account
{
    string username;
    string password;

    public Account(string username, string password)
    {
        this.username = username;
        this.password = password;
    }

    public string getUsername()
    {
        return username;
    }

    public string getPassword()
    {
        return password;
    }
}
