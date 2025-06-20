﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.Constant
{
    public static class ConstantEnum
    {
        public static class RoleID
        {
            public const string ADMIN = "1";
            public const string STUDENT = "2";
            public const string TEACHER = "3";
        }
        public enum TransactionStatus
        {
            PROCCESSING = 1,
            CONFIRMED = 2,
            CANCELED = 3
        }
        public static class PayOsStatus
        {
            public const string PENDING = "PENDING";
            public const string CANCELLED = "CANCELLED";
            public const string PAID = "PAID";
        }
    }
}
