﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PelotonEppSdk.Classes;
using PelotonEppSdk.Models;

namespace PelotonEppSdkTests
{
    [TestClass]
    public class BankAccountCreateTests
    {
        [TestMethod]
        public void TestCreateAccount()
        {
            var createRequest = GetBasicRequest();

            // invent a new bank account token
            createRequest.BankAccount.Token = Guid.NewGuid().ToString().Substring(0, 32);

            var errors = new Collection<string>();
            if (createRequest.TryValidate(errors))
            {
                foreach (var error in errors)
                {
                    Debug.WriteLine(error);
                }
            }
            var result = createRequest.PostAsync().Result;
            Debug.WriteLine(result.Message);
            Debug.WriteLineIf((result.Errors != null && result.Errors.Count >= 1), string.Join("; ", result.Errors ?? new List<string>()));
            Assert.IsTrue(result.Success);
            Assert.AreEqual(0, result.MessageCode);

        }

        private static BankAccountRequest GetBasicRequest()
        {
            //var factory = new RequestFactory(106, "c57cbd1d", "PelonEppSdkTests");
            var factory = new RequestFactory(80, "e9ab9532", "PelonEppSdkTests");
            var createRequest = factory.GetBankAccountCreateRequest();
            createRequest.BankAccount = new BankAccount
            {
                AccountNumber = "1",
                BranchTransitNumber = 1,
                CurrencyCode = "CAD",
                FinancialInstitution = 1,
                Name = "Bank Banktasia",
                Owner = "Unit test SDK",
                Token = "bank account token 1",
                TypeCode = "1"
            };
            createRequest.Document = null; //new Document();
            createRequest.VerifyAccountByDeposit = false;
            
            createRequest.References =
                new List<Reference>
                {
                    new Reference {Name = "String 1", Value = "String2"},
                    new Reference {Name = "String 3", Value = "String4"}
                };
            return (BankAccountRequest)createRequest;
        }
    }
}
