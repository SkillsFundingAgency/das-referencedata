﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.ReferenceData.CharityImport.WebJob.Updater
{
    public interface ICharityImporter
    {
        Task RunUpdate();
    }
}