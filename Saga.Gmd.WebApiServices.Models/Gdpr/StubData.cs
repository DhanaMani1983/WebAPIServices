using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.Models.Customer;
using Saga.Gmd.WebApiServices.Models.Gdpr;

namespace Saga.Gmd.WebApiServices.Models.Gdpr
{
    public class StubData
    {
        public object GetData(GdprResponseType requestType, string responseType, string journey)
        {
            switch (requestType)
            {
                case GdprResponseType.Full:
                    {
                        return GetFullPermissions(journey);
                    }
                case GdprResponseType.Summary:
                    {
                        PermissionSummary summary = new PermissionSummary
                        {
                            ReConsentRequiredCore = true,
                            Hac = false,
                            Source = "Legacy",
                            LastUpdatedDate = DateTime.UtcNow
                        };
                        return summary;
                    }
                case GdprResponseType.Specified:
                    {
                        var specific = new PermissionSpecified
                        {
                            PermissionId = 234567,
                            Source = "CPC",
                            Hac = true,
                            ReConsentRequiredCore = true,
                            LastUpdatedDate = DateTime.UtcNow,
                            LastUpdatedAgentName = "Craig Firth",
                            JourneyType = JourneyType.Online.ToString(),
                            Journey = journey // Journey.MembershipOnline.ToString()
                        };

                        if (responseType.ToLower() == CorePermission.Core.ToString().ToLower())
                        {
                            var insurance = new ChannelFlags
                            {
                                PermissionCategoryDisplayValue = PermissionCategoryDisplayValue.Insurance.ToString(),
                                PermissionCategoryStatus = PermissionCategoryStatus.OptIn.ToString(),
                                ChannelPhoneNoFlag = ChannelFagStatus.NotAsked.ToString(),
                                ChannelPostFlag = ChannelFagStatus.OptIn.ToString(),
                                ChannelSmsFlag = ChannelFagStatus.OptOut.ToString(),
                                ChannelEmailFlag = ChannelFagStatus.NotAsked.ToString(),
                                LastUpdatedDate = DateTime.Now
                            };
                            specific.PermissionCategory.Add(insurance);

                            var travel = new ChannelFlags
                            {
                                PermissionCategoryDisplayValue = PermissionCategoryDisplayValue.Travel.ToString(),
                                PermissionCategoryStatus = PermissionCategoryStatus.OptIn.ToString(),
                                ChannelPhoneNoFlag = ChannelFagStatus.NotAsked.ToString(),
                                ChannelPostFlag = ChannelFagStatus.OptIn.ToString(),
                                ChannelSmsFlag = ChannelFagStatus.OptOut.ToString(),
                                ChannelEmailFlag = ChannelFagStatus.NotAsked.ToString(),
                                LastUpdatedDate = DateTime.Now
                            };
                            specific.PermissionCategory.Add(travel);

                            var money = new ChannelFlags
                            {
                                PermissionCategoryDisplayValue = PermissionCategoryDisplayValue.Money.ToString(),
                                PermissionCategoryStatus = PermissionCategoryStatus.OptIn.ToString(),
                                ChannelPhoneNoFlag = ChannelFagStatus.NotAsked.ToString(),
                                ChannelPostFlag = ChannelFagStatus.OptIn.ToString(),
                                ChannelSmsFlag = ChannelFagStatus.OptOut.ToString(),
                                ChannelEmailFlag = ChannelFagStatus.NotAsked.ToString(),
                                LastUpdatedDate = DateTime.Now
                            };
                            specific.PermissionCategory.Add(money);
                        }
                        else if (responseType.ToLower() == CorePermission.Membership.ToString().ToLower())
                        {
                            var membership = new ChannelFlags
                            {
                                PermissionCategoryDisplayValue = PermissionCategoryDisplayValue.Membership.ToString(),
                                PermissionCategoryStatus = PermissionCategoryStatus.OptIn.ToString(),
                                ChannelPhoneNoFlag = ChannelFagStatus.NotAsked.ToString(),
                                ChannelPostFlag = ChannelFagStatus.OptIn.ToString(),
                                ChannelSmsFlag = ChannelFagStatus.OptOut.ToString(),
                                ChannelEmailFlag = ChannelFagStatus.NotAsked.ToString(),
                                LastUpdatedDate = DateTime.Now
                            };

                            specific.PermissionCategory.Add(membership);
                        }
                        else if (responseType.ToLower() == CorePermission.CoreAndMembership.ToString().ToLower())
                        {
                            var insurance = new ChannelFlags
                            {
                                PermissionCategoryDisplayValue = PermissionCategoryDisplayValue.Insurance.ToString(),
                                PermissionCategoryStatus = PermissionCategoryStatus.OptIn.ToString(),
                                ChannelPhoneNoFlag = ChannelFagStatus.NotAsked.ToString(),
                                ChannelPostFlag = ChannelFagStatus.OptIn.ToString(),
                                ChannelSmsFlag = ChannelFagStatus.OptOut.ToString(),
                                ChannelEmailFlag = ChannelFagStatus.NotAsked.ToString(),
                                LastUpdatedDate = DateTime.Now
                            };
                            specific.PermissionCategory.Add(insurance);

                            var travel = new ChannelFlags
                            {
                                PermissionCategoryDisplayValue = PermissionCategoryDisplayValue.Travel.ToString(),
                                PermissionCategoryStatus = PermissionCategoryStatus.OptIn.ToString(),
                                ChannelPhoneNoFlag = ChannelFagStatus.NotAsked.ToString(),
                                ChannelPostFlag = ChannelFagStatus.OptIn.ToString(),
                                ChannelSmsFlag = ChannelFagStatus.OptOut.ToString(),
                                ChannelEmailFlag = ChannelFagStatus.NotAsked.ToString(),
                                LastUpdatedDate = DateTime.Now
                            };
                            specific.PermissionCategory.Add(travel);

                            var money = new ChannelFlags
                            {
                                PermissionCategoryDisplayValue = PermissionCategoryDisplayValue.Money.ToString(),
                                PermissionCategoryStatus = PermissionCategoryStatus.OptIn.ToString(),
                                ChannelPhoneNoFlag = ChannelFagStatus.NotAsked.ToString(),
                                ChannelPostFlag = ChannelFagStatus.OptIn.ToString(),
                                ChannelSmsFlag = ChannelFagStatus.OptOut.ToString(),
                                ChannelEmailFlag = ChannelFagStatus.NotAsked.ToString(),
                                LastUpdatedDate = DateTime.Now
                            };
                            specific.PermissionCategory.Add(money);

                            var membership = new ChannelFlags
                            {
                                PermissionCategoryDisplayValue = PermissionCategoryDisplayValue.Membership.ToString(),
                                PermissionCategoryStatus = PermissionCategoryStatus.OptIn.ToString(),
                                ChannelPhoneNoFlag = ChannelFagStatus.NotAsked.ToString(),
                                ChannelPostFlag = ChannelFagStatus.OptIn.ToString(),
                                ChannelSmsFlag = ChannelFagStatus.OptOut.ToString(),
                                ChannelEmailFlag = ChannelFagStatus.NotAsked.ToString(),
                                LastUpdatedDate = DateTime.Now
                            };

                            specific.PermissionCategory.Add(membership);
                        }
                        else if (responseType.ToLower() == CorePermission.CoreAndHealth.ToString().ToLower())
                        {
                            var insurance = new ChannelFlags
                            {
                                PermissionCategoryDisplayValue = PermissionCategoryDisplayValue.Insurance.ToString(),
                                PermissionCategoryStatus = PermissionCategoryStatus.OptIn.ToString(),
                                ChannelPhoneNoFlag = ChannelFagStatus.NotAsked.ToString(),
                                ChannelPostFlag = ChannelFagStatus.OptIn.ToString(),
                                ChannelSmsFlag = ChannelFagStatus.OptOut.ToString(),
                                ChannelEmailFlag = ChannelFagStatus.NotAsked.ToString(),
                                LastUpdatedDate = DateTime.Now
                            };
                            specific.PermissionCategory.Add(insurance);

                            var travel = new ChannelFlags
                            {
                                PermissionCategoryDisplayValue = PermissionCategoryDisplayValue.Travel.ToString(),
                                PermissionCategoryStatus = PermissionCategoryStatus.OptIn.ToString(),
                                ChannelPhoneNoFlag = ChannelFagStatus.NotAsked.ToString(),
                                ChannelPostFlag = ChannelFagStatus.OptIn.ToString(),
                                ChannelSmsFlag = ChannelFagStatus.OptOut.ToString(),
                                ChannelEmailFlag = ChannelFagStatus.NotAsked.ToString(),
                                LastUpdatedDate = DateTime.Now
                            };
                            specific.PermissionCategory.Add(travel);

                            var money = new ChannelFlags
                            {
                                PermissionCategoryDisplayValue = PermissionCategoryDisplayValue.Money.ToString(),
                                PermissionCategoryStatus = PermissionCategoryStatus.OptIn.ToString(),
                                ChannelPhoneNoFlag = ChannelFagStatus.NotAsked.ToString(),
                                ChannelPostFlag = ChannelFagStatus.OptIn.ToString(),
                                ChannelSmsFlag = ChannelFagStatus.OptOut.ToString(),
                                ChannelEmailFlag = ChannelFagStatus.NotAsked.ToString(),
                                LastUpdatedDate = DateTime.Now
                            };
                            specific.PermissionCategory.Add(money);

                            var health = new ChannelFlags
                            {
                                PermissionCategoryDisplayValue = PermissionCategoryDisplayValue.Health.ToString(),
                                PermissionCategoryStatus = PermissionCategoryStatus.OptIn.ToString(),
                                ChannelPhoneNoFlag = ChannelFagStatus.NotAsked.ToString(),
                                ChannelPostFlag = ChannelFagStatus.OptIn.ToString(),
                                ChannelSmsFlag = ChannelFagStatus.OptOut.ToString(),
                                ChannelEmailFlag = ChannelFagStatus.NotAsked.ToString(),
                                LastUpdatedDate = DateTime.Now
                            };

                            specific.PermissionCategory.Add(health);
                        }
                        else if (responseType.ToLower() == CorePermission.CoreAndMagazine.ToString().ToLower())
                        {
                            var insurance = new ChannelFlags
                            {
                                PermissionCategoryDisplayValue = PermissionCategoryDisplayValue.Insurance.ToString(),
                                PermissionCategoryStatus = PermissionCategoryStatus.OptIn.ToString(),
                                ChannelPhoneNoFlag = ChannelFagStatus.NotAsked.ToString(),
                                ChannelPostFlag = ChannelFagStatus.OptIn.ToString(),
                                ChannelSmsFlag = ChannelFagStatus.OptOut.ToString(),
                                ChannelEmailFlag = ChannelFagStatus.NotAsked.ToString(),
                                LastUpdatedDate = DateTime.Now
                            };
                            specific.PermissionCategory.Add(insurance);

                            var travel = new ChannelFlags
                            {
                                PermissionCategoryDisplayValue = PermissionCategoryDisplayValue.Travel.ToString(),
                                PermissionCategoryStatus = PermissionCategoryStatus.OptIn.ToString(),
                                ChannelPhoneNoFlag = ChannelFagStatus.NotAsked.ToString(),
                                ChannelPostFlag = ChannelFagStatus.OptIn.ToString(),
                                ChannelSmsFlag = ChannelFagStatus.OptOut.ToString(),
                                ChannelEmailFlag = ChannelFagStatus.NotAsked.ToString(),
                                LastUpdatedDate = DateTime.Now
                            };
                            specific.PermissionCategory.Add(travel);

                            var money = new ChannelFlags
                            {
                                PermissionCategoryDisplayValue = PermissionCategoryDisplayValue.Money.ToString(),
                                PermissionCategoryStatus = PermissionCategoryStatus.OptIn.ToString(),
                                ChannelPhoneNoFlag = ChannelFagStatus.NotAsked.ToString(),
                                ChannelPostFlag = ChannelFagStatus.OptIn.ToString(),
                                ChannelSmsFlag = ChannelFagStatus.OptOut.ToString(),
                                ChannelEmailFlag = ChannelFagStatus.NotAsked.ToString(),
                                LastUpdatedDate = DateTime.Now
                            };
                            specific.PermissionCategory.Add(money);

                            var coreAndMagazine = new ChannelFlags
                            {
                                PermissionCategoryDisplayValue = PermissionCategoryDisplayValue.Magazine.ToString(),
                                PermissionCategoryStatus = PermissionCategoryStatus.OptIn.ToString(),
                                ChannelPhoneNoFlag = ChannelFagStatus.NotAsked.ToString(),
                                ChannelPostFlag = ChannelFagStatus.OptIn.ToString(),
                                ChannelSmsFlag = ChannelFagStatus.OptOut.ToString(),
                                ChannelEmailFlag = ChannelFagStatus.NotAsked.ToString(),
                                LastUpdatedDate = DateTime.Now
                            };

                            specific.PermissionCategory.Add(coreAndMagazine);
                        }
                        else if (responseType.ToLower() == CorePermission.CoreAndRetirementVillage.ToString().ToLower())
                        {
                            var insurance = new ChannelFlags
                            {
                                PermissionCategoryDisplayValue = PermissionCategoryDisplayValue.Insurance.ToString(),
                                PermissionCategoryStatus = PermissionCategoryStatus.OptIn.ToString(),
                                ChannelPhoneNoFlag = ChannelFagStatus.NotAsked.ToString(),
                                ChannelPostFlag = ChannelFagStatus.OptIn.ToString(),
                                ChannelSmsFlag = ChannelFagStatus.OptOut.ToString(),
                                ChannelEmailFlag = ChannelFagStatus.NotAsked.ToString(),
                                LastUpdatedDate = DateTime.Now
                            };
                            specific.PermissionCategory.Add(insurance);

                            var travel = new ChannelFlags
                            {
                                PermissionCategoryDisplayValue = PermissionCategoryDisplayValue.Travel.ToString(),
                                PermissionCategoryStatus = PermissionCategoryStatus.OptIn.ToString(),
                                ChannelPhoneNoFlag = ChannelFagStatus.NotAsked.ToString(),
                                ChannelPostFlag = ChannelFagStatus.OptIn.ToString(),
                                ChannelSmsFlag = ChannelFagStatus.OptOut.ToString(),
                                ChannelEmailFlag = ChannelFagStatus.NotAsked.ToString(),
                                LastUpdatedDate = DateTime.Now
                            };
                            specific.PermissionCategory.Add(travel);

                            var money = new ChannelFlags
                            {
                                PermissionCategoryDisplayValue = PermissionCategoryDisplayValue.Money.ToString(),
                                PermissionCategoryStatus = PermissionCategoryStatus.OptIn.ToString(),
                                ChannelPhoneNoFlag = ChannelFagStatus.NotAsked.ToString(),
                                ChannelPostFlag = ChannelFagStatus.OptIn.ToString(),
                                ChannelSmsFlag = ChannelFagStatus.OptOut.ToString(),
                                ChannelEmailFlag = ChannelFagStatus.NotAsked.ToString(),
                                LastUpdatedDate = DateTime.Now
                            };
                            specific.PermissionCategory.Add(money);

                            var retirementVillages = new ChannelFlags
                            {
                                PermissionCategoryDisplayValue =
                                    PermissionCategoryDisplayValue.RetirementVillages.ToString(),
                                PermissionCategoryStatus = PermissionCategoryStatus.OptIn.ToString(),
                                ChannelPhoneNoFlag = ChannelFagStatus.NotAsked.ToString(),
                                ChannelPostFlag = ChannelFagStatus.OptIn.ToString(),
                                ChannelSmsFlag = ChannelFagStatus.OptOut.ToString(),
                                ChannelEmailFlag = ChannelFagStatus.NotAsked.ToString(),
                                LastUpdatedDate = DateTime.Now
                            };

                            specific.PermissionCategory.Add(retirementVillages);
                        }
                        else
                        {
                            var insurance = new ChannelFlags
                            {
                                PermissionCategoryDisplayValue = PermissionCategoryDisplayValue.Insurance.ToString(),
                                PermissionCategoryStatus = PermissionCategoryStatus.OptIn.ToString(),
                                ChannelPhoneNoFlag = ChannelFagStatus.NotAsked.ToString(),
                                ChannelPostFlag = ChannelFagStatus.OptIn.ToString(),
                                ChannelSmsFlag = ChannelFagStatus.OptOut.ToString(),
                                ChannelEmailFlag = ChannelFagStatus.NotAsked.ToString(),
                                LastUpdatedDate = DateTime.Now
                            };
                            specific.PermissionCategory.Add(insurance);

                            var travel = new ChannelFlags
                            {
                                PermissionCategoryDisplayValue = PermissionCategoryDisplayValue.Travel.ToString(),
                                PermissionCategoryStatus = PermissionCategoryStatus.OptIn.ToString(),
                                ChannelPhoneNoFlag = ChannelFagStatus.NotAsked.ToString(),
                                ChannelPostFlag = ChannelFagStatus.OptIn.ToString(),
                                ChannelSmsFlag = ChannelFagStatus.OptOut.ToString(),
                                ChannelEmailFlag = ChannelFagStatus.NotAsked.ToString(),
                                LastUpdatedDate = DateTime.Now
                            };
                            specific.PermissionCategory.Add(travel);

                            var money = new ChannelFlags
                            {
                                PermissionCategoryDisplayValue = PermissionCategoryDisplayValue.Money.ToString(),
                                PermissionCategoryStatus = PermissionCategoryStatus.OptIn.ToString(),
                                ChannelPhoneNoFlag = ChannelFagStatus.NotAsked.ToString(),
                                ChannelPostFlag = ChannelFagStatus.OptIn.ToString(),
                                ChannelSmsFlag = ChannelFagStatus.OptOut.ToString(),
                                ChannelEmailFlag = ChannelFagStatus.NotAsked.ToString(),
                                LastUpdatedDate = DateTime.Now
                            };
                            specific.PermissionCategory.Add(money);


                            var membership = new ChannelFlags
                            {
                                PermissionCategoryDisplayValue = PermissionCategoryDisplayValue.Membership.ToString(),
                                PermissionCategoryStatus = PermissionCategoryStatus.OptIn.ToString(),
                                ChannelPhoneNoFlag = ChannelFagStatus.NotAsked.ToString(),
                                ChannelPostFlag = ChannelFagStatus.OptIn.ToString(),
                                ChannelSmsFlag = ChannelFagStatus.OptOut.ToString(),
                                ChannelEmailFlag = ChannelFagStatus.NotAsked.ToString(),
                                LastUpdatedDate = DateTime.Now
                            };

                            specific.PermissionCategory.Add(membership);

                            var magazine = new ChannelFlags
                            {
                                PermissionCategoryDisplayValue = PermissionCategoryDisplayValue.Magazine.ToString(),
                                PermissionCategoryStatus = PermissionCategoryStatus.OptIn.ToString(),
                                ChannelPhoneNoFlag = ChannelFagStatus.NotAsked.ToString(),
                                ChannelPostFlag = ChannelFagStatus.OptIn.ToString(),
                                ChannelSmsFlag = ChannelFagStatus.OptOut.ToString(),
                                ChannelEmailFlag = ChannelFagStatus.NotAsked.ToString(),
                                LastUpdatedDate = DateTime.Now
                            };

                            specific.PermissionCategory.Add(magazine);

                            var health = new ChannelFlags
                            {
                                PermissionCategoryDisplayValue = PermissionCategoryDisplayValue.Health.ToString(),
                                PermissionCategoryStatus = PermissionCategoryStatus.OptIn.ToString(),
                                ChannelPhoneNoFlag = ChannelFagStatus.NotAsked.ToString(),
                                ChannelPostFlag = ChannelFagStatus.OptIn.ToString(),
                                ChannelSmsFlag = ChannelFagStatus.OptOut.ToString(),
                                ChannelEmailFlag = ChannelFagStatus.NotAsked.ToString(),
                                LastUpdatedDate = DateTime.Now
                            };

                            specific.PermissionCategory.Add(health);

                            var retirementVillages = new ChannelFlags
                            {
                                PermissionCategoryDisplayValue =
                                    PermissionCategoryDisplayValue.RetirementVillages.ToString(),
                                PermissionCategoryStatus = PermissionCategoryStatus.OptIn.ToString(),
                                ChannelPhoneNoFlag = ChannelFagStatus.NotAsked.ToString(),
                                ChannelPostFlag = ChannelFagStatus.OptIn.ToString(),
                                ChannelSmsFlag = ChannelFagStatus.OptOut.ToString(),
                                ChannelEmailFlag = ChannelFagStatus.NotAsked.ToString(),
                                LastUpdatedDate = DateTime.Now
                            };

                            specific.PermissionCategory.Add(retirementVillages);

                           

                        }

                        CustomerAddress a = new CustomerAddress
                        {
                            HouseName = "Saga",
                            HouseNumber = "007",
                            Street = "Sandgate Road",
                            County = "Kent",
                            City = "Folkstone",
                            Country = "Great Britain",
                            Postcode = "CT04 3XY"
                        };
                        specific.ChannelPostalAddressList.Add(a);
                        CustomerAddress b = new CustomerAddress
                        {
                            HouseName = "Saga 1",
                            HouseNumber = "0077",
                            Street = "Sandgate Road",
                            City = "Folkstone",
                            County = "Kent",
                            Country = "Great Britain",
                            Postcode = "CT04 3XY"
                        };
                        specific.ChannelPostalAddressList.Add(b);

                        List<string> emailList = new List<string> { "john@gmail.com", "john@ssaga.co.uk", "John.doe@saga.co.uk" };
                        specific.ChannelEmailList.AddRange(emailList);

                        List<string> sms = new List<string> { "1209839389", "2393039409" };
                        specific.ChannelSmsNoList.AddRange(sms);

                        List<string> phonelist = new List<string> { "020-3094-3939", "066-3039-309" };
                        specific.ChannelPhoneNoList.AddRange(phonelist);

                        specific.ChannelPostalAddress = specific.ChannelPostalAddressList.FirstOrDefault();
                        specific.ChannelEmailAddress = specific.ChannelEmailList.FirstOrDefault();
                        specific.ChannelSmsNo = specific.ChannelSmsNoList.FirstOrDefault();
                        specific.ChannelPhoneNo = specific.ChannelPhoneNoList.FirstOrDefault();

                        return specific;
                    }
            }

            return null;
        }

        private object GetFullPermissions(string journey)
        {
            var permission = new PermissionFull
            {
                PermissionId = 12345,
                Source = "CPC",
                Hac = true,
                ReConsentRequiredCore = true,
                JourneyType = JourneyType.Online.ToString(),
                Journey = journey,//Journey.Insurance.ToString()
                LastUpdatedDate = DateTime.UtcNow,
                LastUpdatedAgentName = "Bob Miller"

            };


            var magazine = new ChannelFlags
            {

                PermissionCategoryDisplayValue = PermissionCategoryDisplayValue.Magazine.ToString(),
                PermissionCategoryStatus = PermissionCategoryStatus.OptOut.ToString(),
                ChannelPhoneNoFlag = ChannelFagStatus.NotAsked.ToString(),
                ChannelPostFlag = ChannelFagStatus.OptIn.ToString(),
                ChannelSmsFlag = ChannelFagStatus.OptOut.ToString(),
                ChannelEmailFlag = ChannelFagStatus.NotAsked.ToString(),
                LastUpdatedDate = DateTime.Now
            };
            permission.PermissionCategory.Add(magazine);

            var travel = new ChannelFlags
            {

                PermissionCategoryDisplayValue = PermissionCategoryDisplayValue.Magazine.ToString(),
                PermissionCategoryStatus = PermissionCategoryStatus.OptOut.ToString(),
                ChannelPhoneNoFlag = ChannelFagStatus.NotAsked.ToString(),
                ChannelPostFlag = ChannelFagStatus.OptIn.ToString(),
                ChannelSmsFlag = ChannelFagStatus.OptOut.ToString(),
                ChannelEmailFlag = ChannelFagStatus.NotAsked.ToString(),
                LastUpdatedDate = DateTime.Now
            };
            permission.PermissionCategory.Add(travel);


            var health = new ChannelFlags
            {
                PermissionCategoryDisplayValue = PermissionCategoryDisplayValue.Health.ToString(),
                PermissionCategoryStatus = PermissionCategoryStatus.OptIn.ToString(),
                ChannelPhoneNoFlag = ChannelFagStatus.NotAsked.ToString(),
                ChannelPostFlag = ChannelFagStatus.OptIn.ToString(),
                ChannelSmsFlag = ChannelFagStatus.OptOut.ToString(),
                ChannelEmailFlag = ChannelFagStatus.NotAsked.ToString(),
                LastUpdatedDate = DateTime.Now
            };
            permission.PermissionCategory.Add(health);

            var insurance = new ChannelFlags
            {
                PermissionCategoryDisplayValue = PermissionCategoryDisplayValue.Insurance.ToString(),
                PermissionCategoryStatus = PermissionCategoryStatus.OptIn.ToString(),
                ChannelPhoneNoFlag = ChannelFagStatus.NotAsked.ToString(),
                ChannelPostFlag = ChannelFagStatus.OptIn.ToString(),
                ChannelSmsFlag = ChannelFagStatus.OptOut.ToString(),
                ChannelEmailFlag = ChannelFagStatus.NotAsked.ToString(),
                LastUpdatedDate = DateTime.Now
            };
            permission.PermissionCategory.Add(insurance);

            var membership = new ChannelFlags
            {
                PermissionCategoryDisplayValue = PermissionCategoryDisplayValue.Membership.ToString(),
                PermissionCategoryStatus = PermissionCategoryStatus.OptIn.ToString(),
                ChannelPhoneNoFlag = ChannelFagStatus.NotAsked.ToString(),
                ChannelPostFlag = ChannelFagStatus.OptIn.ToString(),
                ChannelSmsFlag = ChannelFagStatus.OptOut.ToString(),
                ChannelEmailFlag = ChannelFagStatus.NotAsked.ToString(),
                LastUpdatedDate = DateTime.Now
            };
            permission.PermissionCategory.Add(membership);

            var money = new ChannelFlags
            {
                PermissionCategoryDisplayValue = PermissionCategoryDisplayValue.Money.ToString(),
                PermissionCategoryStatus = PermissionCategoryStatus.OptIn.ToString(),
                ChannelPhoneNoFlag = ChannelFagStatus.NotAsked.ToString(),
                ChannelPostFlag = ChannelFagStatus.OptIn.ToString(),
                ChannelSmsFlag = ChannelFagStatus.OptOut.ToString(),
                ChannelEmailFlag = ChannelFagStatus.NotAsked.ToString(),
                LastUpdatedDate = DateTime.Now
            };
            permission.PermissionCategory.Add(money);

            var retirementVillages = new ChannelFlags
            {
                PermissionCategoryDisplayValue = PermissionCategoryDisplayValue.RetirementVillages.ToString(),
                PermissionCategoryStatus = PermissionCategoryStatus.NotAsked.ToString(),
                ChannelPhoneNoFlag = ChannelFagStatus.NotAsked.ToString(),
                ChannelPostFlag = ChannelFagStatus.OptIn.ToString(),
                ChannelSmsFlag = ChannelFagStatus.OptOut.ToString(),
                ChannelEmailFlag = ChannelFagStatus.NotAsked.ToString(),
                LastUpdatedDate = DateTime.Now
            };
            permission.PermissionCategory.Add(retirementVillages);



            CustomerAddress a = new CustomerAddress
            {
                HouseName = "Saga",
                HouseNumber = "007",
                Street = "Sandgate Road",
                County = "Kent",
                City = "Folkstone",
                Country = "Great Britain",
                Postcode = "CT04 3XY"
            };
            permission.ChannelPostalAddressList.Add(a);

            CustomerAddress b = new CustomerAddress
            {
                HouseName = "Saga 1",
                HouseNumber = "0077",
                Street = "Sandgate Road",
                City = "Folkstone",
                County = "Kent",
                Country = "Great Britain",
                Postcode = "CT04 3XY"
            };
            permission.ChannelPostalAddressList.Add(b);


            List<string> emailList = new List<string>();
            emailList.Add("john@gmail.com");
            emailList.Add("john@ssaga.co.uk");
            emailList.Add("John.doe@saga.co.uk");
            permission.ChannelEmailList.AddRange(emailList);

            List<string> sms = new List<string> { "1209839389", "2393039409" };
            permission.ChannelSmsNoList.AddRange(sms);

            List<string> phonelist = new List<string> { "020-3094-3939", "066-3039-309" };
            permission.ChannelPhoneNoList.AddRange(phonelist);



            permission.ChannelPostalAddress = permission.ChannelPostalAddressList.FirstOrDefault();
            permission.ChannelEmailAddress = permission.ChannelEmailList.FirstOrDefault();
            permission.ChannelSmsNo = permission.ChannelSmsNoList.FirstOrDefault();
            permission.ChannelPhoneNo = permission.ChannelPhoneNoList.FirstOrDefault();
            return permission;
        }
    }
}
