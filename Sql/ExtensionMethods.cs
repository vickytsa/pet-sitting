﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VickyTsao.PetCare.Objects;

namespace VickyTsao.PetCare.Sql
{
    public static class ExtensionMethods
    {
        public static T SafeGetValue<T>(this SqlDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
                return reader.GetFieldValue<T>(colIndex);
            return default(T);
        }


    }
}
