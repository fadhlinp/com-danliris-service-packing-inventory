﻿using Com.Danliris.Service.Packing.Inventory.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Com.Danliris.Service.Packing.Inventory.Infrastructure.Repositories.DyeingPrintingAreaMovement;
using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Com.Danliris.Service.Packing.Inventory.Infrastructure.IdentityProvider;
using Com.Danliris.Service.Packing.Inventory.Data.Models;
using System.Linq;
using Com.Danliris.Service.Packing.Inventory.Data.Models.FabricQualityControl;
using Com.Moonlay.Models;
using System.Collections.Generic;
using Com.Danliris.Service.Packing.Inventory.Test.DataUtils;

namespace Com.Danliris.Service.Packing.Inventory.Test.Repositories
{
    public class DyeingPrintingAreaMovementRepositoryTest
        : BaseRepositoryTest<PackingInventoryDbContext, DyeingPrintingAreaMovementRepository, DyeingPrintingAreaMovementModel, DyeingPrintingAreaMovementDataUtil>
    {
        private const string ENTITY = "DyeingPrintintAreaMovement";

        public DyeingPrintingAreaMovementRepositoryTest() : base(ENTITY)
        {

        }

        [Fact]
        public async Task Should_Success_InsertYard()
        {
            string testName = GetCurrentMethod();
            var dbContext = DbContext(testName);
            var serviceProvider = GetServiceProviderMock(dbContext).Object;
            var repo = new DyeingPrintingAreaMovementRepository(dbContext, GetServiceProviderMock(dbContext).Object);
            var data = DataUtil(repo, dbContext).GetYardModel();
            var result = await repo.InsertAsync(data);
            Assert.NotEqual(0, result);
        }

        [Fact]
        public async Task Should_Exception_Delete()
        {
            string testName = GetCurrentMethod();
            var dbContext = DbContext(testName);

            var repo = new DyeingPrintingAreaMovementRepository(dbContext, GetServiceProviderMock(dbContext).Object);
            var data = await DataUtil(repo, dbContext).GetTestData();

            var fqcModel = new FabricQualityControlModel("code", DateTimeOffset.UtcNow, "area", false, data.Id, data.BonNo, data.ProductionOrderNo, "machine",
                "op", 1, 1, new List<FabricGradeTestModel>());
            fqcModel.FlagForCreate("test", "test");
            var datafqc = dbContext.Add(fqcModel);
            await dbContext.SaveChangesAsync();

            await Assert.ThrowsAnyAsync<Exception>(() => repo.DeleteAsync(data.Id));
        }

        [Fact]
        public async Task Should_Success_GetDbSet()
        {
            string testName = GetCurrentMethod();
            var dbContext = DbContext(testName);

            var repo = new DyeingPrintingAreaMovementRepository(dbContext, GetServiceProviderMock(dbContext).Object);
            var data = await DataUtil(repo, dbContext).GetTestData();
            var result = repo.GetDbSet();

            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task Should_Success_ReadAllIgnoreQueryFilter()
        {
            string testName = GetCurrentMethod();
            var dbContext = DbContext(testName);

            var repo = new DyeingPrintingAreaMovementRepository(dbContext, GetServiceProviderMock(dbContext).Object);
            var data = await DataUtil(repo, dbContext).GetTestData();
            var result = repo.ReadAllIgnoreQueryFilter();

            Assert.NotEmpty(result);
        }

        [Fact]
        public virtual async Task Should_Success_InsertFromTransit()
        {
            string testName = GetCurrentMethod();
            var dbContext = DbContext(testName);

            var serviceProvider = GetServiceProviderMock(dbContext).Object;
            var repo = new DyeingPrintingAreaMovementRepository(dbContext, GetServiceProviderMock(dbContext).Object);
            var data = await DataUtil(repo, dbContext).GetTestData();
            var result = await repo.InsertFromTransitAsync(data.Id, data.Shift, data.Date, "Transit", "rem", new DyeingPrintingAreaMovementHistoryModel(data.Date, "Transit", data.Shift, AreaEnum.TRANSIT));

            Assert.NotEqual(0, result);
        }

        [Fact]
        public virtual async Task Should_Success_DeleteFromTransit()
        {
            string testName = GetCurrentMethod();
            var dbContext = DbContext(testName);

            var serviceProvider = GetServiceProviderMock(dbContext).Object;
            var repo = new DyeingPrintingAreaMovementRepository(dbContext, GetServiceProviderMock(dbContext).Object);
            var data = await DataUtil(repo, dbContext).GetTestData();
            await repo.InsertFromTransitAsync(data.Id, data.Shift, data.Date, "Transit", "rem", new DyeingPrintingAreaMovementHistoryModel(data.Date, "Transit", data.Shift, AreaEnum.TRANSIT));
            var result = await repo.DeleteFromTransitAsync(data.Id);

            Assert.NotEqual(0, result);
        }

        [Fact]
        public virtual async Task Should_Success_UpdateFromTransit()
        {
            string testName = GetCurrentMethod() + "UpdateFromTransit";
            var dbContext = DbContext(testName);

            var serviceProvider = GetServiceProviderMock(dbContext).Object;
            var repo = new DyeingPrintingAreaMovementRepository(dbContext, GetServiceProviderMock(dbContext).Object);
            var data = await DataUtil(repo, dbContext).GetTestData();
            await repo.InsertFromTransitAsync(data.Id, null, data.Date, "Transit", null, new DyeingPrintingAreaMovementHistoryModel(data.Date, "Transit", null, AreaEnum.TRANSIT));
            var result = await repo.UpdateFromTransitAsync(data.Id, data.Shift, "rem");

            Assert.NotEqual(0, result);
        }

        [Fact]
        public virtual async Task Should_Success_UpdateFromFQC()
        {
            string testName = GetCurrentMethod() + "UpdateFromTransit";
            var dbContext = DbContext(testName);

            var serviceProvider = GetServiceProviderMock(dbContext).Object;
            var repo = new DyeingPrintingAreaMovementRepository(dbContext, GetServiceProviderMock(dbContext).Object);
            var data = await DataUtil(repo, dbContext).GetTestData();
            data.SetStatus("ok", "user", "agent");
            var result = await repo.UpdateFromFabricQualityControlAsync(data.Id, "grade", true);

            Assert.NotEqual(0, result);
        }
    }
}
