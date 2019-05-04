
namespace FlowNs___cc_11b665da_85b0_4b5c_859b_0c63b0c8a4e3
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime;

    //#id 11b665da-85b0-4b5c-859b-0c63b0c8a4e3
    //#Container
    [Coreflow.Objects.FlowIdentifierAttribute("11b665da-85b0-4b5c-859b-0c63b0c8a4e3")]
    public class Flow___cc_11b665da_85b0_4b5c_859b_0c63b0c8a4e3 : Coreflow.Interfaces.ICompiledFlow
    {

        public Guid InstanceId = Guid.NewGuid();


        public void SetArguments(IDictionary<string, object> pArguments)
        {
        }

        public IDictionary<string, object> GetArguments()
        {
            Dictionary<string, object> ret = new Dictionary<string, object>();
            return ret;
        }

        Guid Coreflow.Interfaces.ICompiledFlow.InstanceId => InstanceId;

        public void Run()
        {
            //#id e5f3643b-5c38-4fc3-bdd3-14f01c2dcfcd
            //#Container
            {
                //#id f25adaa1-7ff8-4b5b-a9e4-73d3b321d102
                Coreflow.Activities.Filesystem.FileReadAllText __cc_f25adaa1_7ff8_4b5b_a9e4_73d3b321d102 = new Coreflow.Activities.Filesystem.FileReadAllText();
                //#id ebaf5e7a-f027-447c-b630-cb70ba27aafa
                Coreflow.ConsoleWriteLineActivity __cc_ebaf5e7a_f027_447c_b630_cb70ba27aafa = new Coreflow.ConsoleWriteLineActivity();

                {
                    //#id f25adaa1-7ff8-4b5b-a9e4-73d3b321d102
                    //#id c10a59dd-fda8-4d69-8d64-6644d64ddd0d
                    var test
                     =
                    __cc_f25adaa1_7ff8_4b5b_a9e4_73d3b321d102.Execute(

                    //#id 84478ef0-8c3f-4fba-a723-56005ee3d872
                    @"C:\Users\Manuel\Documents\Arma 3\stats.Arma3.json"
                    );


                    //#id ebaf5e7a-f027-447c-b630-cb70ba27aafa
                    __cc_ebaf5e7a_f027_447c_b630_cb70ba27aafa.Execute(

                    //#id 58f10f93-018c-4f7b-83e0-5ed883a55cda
                    test
                    );


                    /* !!! --------------------------------------------------------------- !!!*/
                }
            }
        } /* Run */
    } /* Class */
} /* Namespace */

namespace FlowNs___cc_a1bfd8b5_d453_4fe0_9825_59cd67f02d5e
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime;

    //#id a1bfd8b5-d453-4fe0-9825-59cd67f02d5e
    //#Container
    [Coreflow.Objects.FlowIdentifierAttribute("a1bfd8b5-d453-4fe0-9825-59cd67f02d5e")]
    public class Flow___cc_a1bfd8b5_d453_4fe0_9825_59cd67f02d5e : Coreflow.Interfaces.ICompiledFlow
    {

        public Guid InstanceId = Guid.NewGuid();


        public void SetArguments(IDictionary<string, object> pArguments)
        {
        }

        public IDictionary<string, object> GetArguments()
        {
            Dictionary<string, object> ret = new Dictionary<string, object>();
            return ret;
        }

        Guid Coreflow.Interfaces.ICompiledFlow.InstanceId => InstanceId;

        public void Run()
        {
            //#id b88aa065-df8d-4ba1-8a4b-218c37cc90ec
            //#Container
            {
                //#id 9079bbfa-9d19-42b2-9b18-a648c3d635c9
                Coreflow.ConsoleWriteLineActivity __cc_9079bbfa_9d19_42b2_9b18_a648c3d635c9 = new Coreflow.ConsoleWriteLineActivity();

                {
                    //#id 234624f1-a889-4653-967a-0463b60e8aeb
                    System.Diagnostics.Debugger.Launch();
                    System.Console.WriteLine("Wait for debugger...");
                    while (!System.Diagnostics.Debugger.IsAttached)
                    {
                        System.Threading.Thread.Sleep(500);
                    }
                    System.Diagnostics.Debugger.Break();


                    //#id 9079bbfa-9d19-42b2-9b18-a648c3d635c9
                    __cc_9079bbfa_9d19_42b2_9b18_a648c3d635c9.Execute(

                    //#id c86cb032-876f-482e-9020-77b344724117
                    "init flow START"
                    );


                    //#id e1900cfb-0a1f-4961-8f70-caea76ac97b9
                    //#Container
                    {
                        for (
                        //#id 1facb0e1-dfed-4a6d-b64b-1e9de41c5215
                        ; ;
                        )

                        {
                            //#id 95d9c26c-e088-4192-9ca3-12dbf99a8207
                            //#Container
                            {
                                //#id 9e7229e3-bd23-4e88-a624-c0904306f120
                                Coreflow.SleepActivity __cc_9e7229e3_bd23_4e88_a624_c0904306f120 = new Coreflow.SleepActivity();

                                {
                                    var __cc_b6dcf18f_b2a1_44dd_9018_b773c48e1351 = new FlowNs___cc_c90ee65b_2726_4f0f_b866_053f3c138cf9.Flow___cc_c90ee65b_2726_4f0f_b866_053f3c138cf9();
                                    var __cc_b6dcf18f_b2a1_44dd_9018_b773c48e1351_1 = new Dictionary<string, object>();
                                    __cc_b6dcf18f_b2a1_44dd_9018_b773c48e1351.SetArguments(__cc_b6dcf18f_b2a1_44dd_9018_b773c48e1351_1);
                                    __cc_b6dcf18f_b2a1_44dd_9018_b773c48e1351.Run();


                                    //#id 9e7229e3-bd23-4e88-a624-c0904306f120
                                    __cc_9e7229e3_bd23_4e88_a624_c0904306f120.Execute(

                                    //#id a90ef27f-e682-456a-bd67-6241d75a2bd5
                                    1000 * 60 * 30
                                    );


                                }
                            }


                        }
                    }


                    /* !!! --------------------------------------------------------------- !!!*/
                }
            }
        } /* Run */
    } /* Class */
} /* Namespace */

namespace FlowNs___cc_aa0aba2a_6b96_44ff_ba72_1d77869b7219
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime;

    //#id aa0aba2a-6b96-44ff-ba72-1d77869b7219
    //#Container
    [Coreflow.Objects.FlowIdentifierAttribute("aa0aba2a-6b96-44ff-ba72-1d77869b7219")]
    public class Flow___cc_aa0aba2a_6b96_44ff_ba72_1d77869b7219 : Coreflow.Interfaces.ICompiledFlow
    {

        public Guid InstanceId = Guid.NewGuid();


        public void SetArguments(IDictionary<string, object> pArguments)
        {
        }

        public IDictionary<string, object> GetArguments()
        {
            Dictionary<string, object> ret = new Dictionary<string, object>();
            return ret;
        }

        Guid Coreflow.Interfaces.ICompiledFlow.InstanceId => InstanceId;

        public void Run()
        {
            //#id 52697f7a-919c-4536-a791-22a74d784d5f
            //#Container
            {

                {
                    //#id d31eb2e8-c3cf-41a2-b782-7d5580418baf
                    //#Container
                    {
                        //#id 9bf4d5f6-c6f2-4b1d-a10a-cf55fd48f9ec
                        Coreflow.ConsoleWriteLineActivity __cc_9bf4d5f6_c6f2_4b1d_a10a_cf55fd48f9ec = new Coreflow.ConsoleWriteLineActivity();
                        if (
                        //#id 981e2de4-399c-4fec-ae30-d79d07bc6a5d
                        DateTime.Now.Hour == 0
                        )

                        {
                            //#id 9bf4d5f6-c6f2-4b1d-a10a-cf55fd48f9ec
                            __cc_9bf4d5f6_c6f2_4b1d_a10a_cf55fd48f9ec.Execute(

                            //#id bfd394a4-bdbd-41e9-af2a-f5a124c4b6fe
                            "Es ist Mitternacht!"
                            );


                        }
                        else
                        {
                            //#id 55c16499-dcce-441d-b658-c360fe0ce9be
                            __cc_9bf4d5f6_c6f2_4b1d_a10a_cf55fd48f9ec.Execute(

                            //#id c8ab6040-5909-460c-b0d1-738813eb523d
                            "Aktuelle Stunde: " + DateTime.Now.Hour
                            );


                        }


                    }


                    /* !!! --------------------------------------------------------------- !!!*/
                }
            }
        } /* Run */
    } /* Class */
} /* Namespace */

namespace FlowNs___cc_c90ee65b_2726_4f0f_b866_053f3c138cf9
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime;
    using System.Net.Mail;

    //#id c90ee65b-2726-4f0f-b866-053f3c138cf9
    //#Container
    [Coreflow.Objects.FlowIdentifierAttribute("c90ee65b-2726-4f0f-b866-053f3c138cf9")]
    public class Flow___cc_c90ee65b_2726_4f0f_b866_053f3c138cf9 : Coreflow.Interfaces.ICompiledFlow
    {

        public Guid InstanceId = Guid.NewGuid();

        public System.Int32 percent = 20;

        public void SetArguments(IDictionary<string, object> pArguments)
        {
            if (pArguments.ContainsKey("percent")) { percent = (System.Int32)pArguments["percent"]; }
        }

        public IDictionary<string, object> GetArguments()
        {
            Dictionary<string, object> ret = new Dictionary<string, object>();
            ret.Add("percent", percent);
            return ret;
        }

        Guid Coreflow.Interfaces.ICompiledFlow.InstanceId => InstanceId;

        public void Run()
        {
            //#id d27f4568-23c9-4819-b87a-715a1a283aa4
            //#Container
            {
                //#id 7cf34c3f-ee02-4091-88e6-0bfa01af1d16
                Coreflow.ConsoleWriteLineActivity __cc_7cf34c3f_ee02_4091_88e6_0bfa01af1d16 = new Coreflow.ConsoleWriteLineActivity();
                //#id 66a078a3-f012-4bdd-a4cd-e376771e10db
                Coreflow.Activities.Filesystem.CheckDisksFreePercent __cc_66a078a3_f012_4bdd_a4cd_e376771e10db = new Coreflow.Activities.Filesystem.CheckDisksFreePercent();

                {
                    //#id 7cf34c3f-ee02-4091-88e6-0bfa01af1d16
                    __cc_7cf34c3f_ee02_4091_88e6_0bfa01af1d16.Execute(

                    //#id 3fc40195-b48f-4bcb-b029-3e0dee30ced0
                    "start check disk percent: " + percent
                    );


                    //#id 66a078a3-f012-4bdd-a4cd-e376771e10db
                    __cc_66a078a3_f012_4bdd_a4cd_e376771e10db.Execute(

                    //#id d9a5299e-5cd3-454f-b653-8723ce463c83
                    percent,
                    //#id 28373b0c-eb59-4c02-9c65-3c20d548a5fb
                    out var disksFree
                    );


                    //#id 26e22901-37ab-4500-a2d7-63fd3510a3b6
                    //#Container
                    {
                        //#id a565a9d7-0eb2-4097-a3a6-20b50977e792
                        Coreflow.Activities.Common.SendEmail __cc_a565a9d7_0eb2_4097_a3a6_20b50977e792 = new Coreflow.Activities.Common.SendEmail();
                        if (
                        //#id ed1d6a08-916b-4f98-a364-b59e358e3b6b
                        !disksFree
                        )

                        {
                            //#id ba43a631-336b-49ba-bb57-1fca875fc746
                            __cc_7cf34c3f_ee02_4091_88e6_0bfa01af1d16.Execute(

                            //#id 50a6ccbe-2fcb-459a-86d5-48504a4ade07
                            "disks too low"
                            );


                            //#id a565a9d7-0eb2-4097-a3a6-20b50977e792
                            __cc_a565a9d7_0eb2_4097_a3a6_20b50977e792.Execute(

                            //#id 4fe0f794-e1aa-407c-82dd-d2c0c746ae38
                            "eco-server.host",
                            //#id 3efe2a51-b501-4e1d-b818-8b8c40d0b538
                            25,
                            //#id 304ae243-c42e-429b-9fca-53617cd3938e
                            false,
                            //#id a1427dfd-4495-4821-9888-da53df683f2f
                            "",
                            //#id b592adab-c012-41ef-9808-628180fae9ad
                            "",
                            //#id e971ed0a-6066-4e05-b13e-a0b6d999aeef
                            "Flow: Disks almost full",
                            //#id 932850df-9992-45e6-ac5c-acc9356d144c
                            "Warning: Disk full",
                            //#id c0825e4f-988e-4d66-94b7-8f3574c38d02
                            Enumerable.Empty<System.Net.Mail.Attachment>(),
                            //#id 6b6cf57d-e989-4c0e-92e8-18b772e3540a
                            new MailAddress(""),
                            //#id 63ac6a70-99be-441c-878a-8882c3276649
                            new[] { new MailAddress("") },
                            //#id c607770b-9815-4179-a45b-26c2bc233b3f
                            Enumerable.Empty<System.Net.Mail.MailAddress>(),
                            //#id 71a00aa5-078e-4232-97be-9917f6aaa803
                            Enumerable.Empty<System.Net.Mail.MailAddress>()
                            );


                        }
                    }


                    /* !!! --------------------------------------------------------------- !!!*/
                }
            }
        } /* Run */
    } /* Class */
} /* Namespace */
