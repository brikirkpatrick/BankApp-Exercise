﻿using BankAppLibrary.Accounts;
using System;

namespace BankAppLibrary
{
    public class CorporateInvestmentAcc : InvestmentAccount
    {
        public CorporateInvestmentAcc(string ownerName, Guid id, IBank bank) : base(ownerName, id, bank, InvestmentAccType.Corporate)
        {
        }
    }
}
