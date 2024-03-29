﻿namespace AccountManager.Models.Accounts
{
    public class Account
    {
        public Account(string id, string site, string password, string description)
        {
            Id = id;
            Site = site;
            Password = password;
            Description = description;
        }

        public string Id { get; }
        public string Site { get; }
        public string Password { get; }
        public string Description { get; }
    }
}
