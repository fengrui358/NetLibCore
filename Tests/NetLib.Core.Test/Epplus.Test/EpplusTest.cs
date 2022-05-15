using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using FrHello.NetLib.Core.Attributes;
using FrHello.NetLib.Core.Framework;
using FrHello.NetLib.Core.Framework.Excel.Attributes;
using FrHello.NetLib.Core.Interfaces;
using NetLib.Core.Test.ConstString;
using OfficeOpenXml;
using Xunit;

namespace NetLib.Core.Test.Epplus.Test
{
    /// <summary>
    /// EpplusTest
    /// </summary>
    public class EpplusTest
    {
        /// <summary>
        /// FillDatasTest
        /// </summary>
        [Fact]
        public void FillDatasTest()
        {
            var resourceNames = typeof(EpplusTest).Assembly.GetManifestResourceNames();
            var testExcel = typeof(EpplusTest).Assembly.GetManifestResourceStream(resourceNames.First());

            using var excelPackage = new ExcelPackage(testExcel);
            var mockAdministrativeRegion = excelPackage.FillDatas<MockAdministrativeRegion>().ToList();
            var mockOrganizations = excelPackage.FillDatas<MockOrganization>();
            var mockDepartments = excelPackage.FillDatas<MockDepartment>();
            var mockPersons = excelPackage.FillDatas<MockPerson>();

            Assert.True(mockAdministrativeRegion.Any());
            Assert.Equal(2, mockOrganizations.Count());
            Assert.Equal(2, mockDepartments.Count());
            Assert.Equal(3, mockPersons.Count());
        }

        /// <summary>
        /// WriteDatasTest
        /// </summary>
        [Fact(Skip = TestStrings.ManuallyExcuteTip)]
        public void WriteDatasTest()
        {
            var resourceNames = typeof(EpplusTest).Assembly.GetManifestResourceNames();
            var testExcel = typeof(EpplusTest).Assembly.GetManifestResourceStream(resourceNames.First());

            using var excelPackage = new ExcelPackage(testExcel);
            var mockAdministrativeRegion = excelPackage.FillDatas<MockAdministrativeRegion>().ToList();
            var mockOrganizations = excelPackage.FillDatas<MockOrganization>();
            var mockDepartments = excelPackage.FillDatas<MockDepartment>();
            var mockPersons = excelPackage.FillDatas<MockPerson>();

            var outputExcelPath = System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                $"{Guid.NewGuid():N}.xlsx");

            Extensions.WriteDatas(mockAdministrativeRegion, outputExcelPath).GetAwaiter().GetResult();
            Extensions.WriteDatas(mockOrganizations, outputExcelPath).GetAwaiter().GetResult();
            Extensions.WriteDatas(mockDepartments, outputExcelPath).GetAwaiter().GetResult();
            Extensions.WriteDatas(mockPersons, outputExcelPath).GetAwaiter().GetResult();

            //测试追加写入
            Extensions.WriteDatas(mockAdministrativeRegion, outputExcelPath).GetAwaiter().GetResult();
        }

        /// <summary>
        /// AppendRowTest
        /// </summary>
        [Fact(Skip = TestStrings.ManuallyExcuteTip)]
        public void AppendRowTest()
        {
            var resourceNames = typeof(EpplusTest).Assembly.GetManifestResourceNames();
            using var testExcel = typeof(EpplusTest).Assembly.GetManifestResourceStream(resourceNames.First(s => s.EndsWith("xlsx")));

            using var excelPackage = new ExcelPackage(testExcel);
            var mockAdministrativeRegion = excelPackage.FillDatas<MockAdministrativeRegion>().ToList();
            var mockOrganizations = excelPackage.FillDatas<MockOrganization>();
            var mockDepartments = excelPackage.FillDatas<MockDepartment>();
            var mockPersons = excelPackage.FillDatas<MockPerson>();

            var outputExcelPath = System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                $"{Guid.NewGuid():N}.xlsx");

            foreach (var administrativeRegion in mockAdministrativeRegion)
            {
                Extensions.AppendRow(outputExcelPath, administrativeRegion).GetAwaiter().GetResult();
            }

            foreach (var mockOrganization in mockOrganizations)
            {
                Extensions.AppendRow(outputExcelPath, mockOrganization).GetAwaiter().GetResult();
            }

            foreach (var mockDepartment in mockDepartments)
            {
                Extensions.AppendRow(outputExcelPath, mockDepartment).GetAwaiter().GetResult();
            }

            foreach (var mockPerson in mockPersons)
            {
                Extensions.AppendRow(outputExcelPath, mockPerson).GetAwaiter().GetResult();
            }

            foreach (var administrativeRegion in mockAdministrativeRegion)
            {
                Extensions.AppendRow(outputExcelPath, administrativeRegion).GetAwaiter().GetResult();
            }
        }

        /// <summary>
        /// InsertImageTest
        /// </summary>
        [Fact(Skip = TestStrings.ManuallyExcuteTip)]
        public void InsertImageTest()
        {
            var resourceNames = typeof(EpplusTest).Assembly.GetManifestResourceNames();
            using var testExcel = typeof(EpplusTest).Assembly.GetManifestResourceStream(resourceNames.First(s => s.EndsWith("xlsx")));
            using var testJpg = typeof(EpplusTest).Assembly.GetManifestResourceStream(resourceNames.First(s => s.EndsWith("jpg")));

            using var excelPackage = new ExcelPackage(testExcel);
            var mockPersons = excelPackage.FillDatas<MockPerson>();

            var bytes = new byte[testJpg.Length];
            testJpg.Read(bytes, 0, bytes.Length);
            var workSheet = excelPackage.Workbook.Worksheets.FirstOrDefault(s => s.Name == "Usuario(用户管理)");
            for (int i = 0; i < mockPersons.Count(); i++)
            {
                // 在人员部分测试插入图片
                workSheet.Row(i + 2).Height = 80;
                Extensions.InsertImage(workSheet, bytes, i + 1, 4, true);
            }

            var outputExcelPath = System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                $"{Guid.NewGuid():N}.xlsx");
            using var fileStream = new FileStream(outputExcelPath, FileMode.Create);
            excelPackage.SaveAs(fileStream);
        }
    }

    /// <summary>
    /// 机构
    /// </summary>
    [Sheet("Institución(机构/名称)")]
    internal class MockOrganization
    {
        /// <summary>
        /// 机构名称
        /// </summary>
        [SheetColumn("Institución(机构名称)")]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [SheetColumn("Descripción(机构概述)")]
        public string Description { get; set; }
    }

    /// <summary>
    /// 部门
    /// </summary>
    [Sheet("Departamento(部门)")]
    internal class MockDepartment
    {
        /// <summary>
        /// 名称
        /// </summary>
        [SheetColumn("Nombre(部门名称)")]
        public string Name { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [SheetColumn("Teléfono(联系电话)")]
        public string TelePhone { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
        [SheetColumn("Fax(传真号码)")]
        public string Fax { get; set; }

        /// <summary>
        /// 部门邮箱
        /// </summary>
        [SheetColumn("Email(部门联系邮箱)")]
        public string Email { get; set; }

        /// <summary>
        /// 所在地区
        /// </summary>
        [SheetColumn("Ubicación(所在地区)")]
        public string City { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [SheetColumn("Dirección(地址)")]
        public string Location { get; set; }

        /// <summary>
        /// 所属机构
        /// </summary>
        [SheetColumn("Institución(所属机构)")]
        public string Organization { get; set; }

        /// <summary>
        /// 内线电话
        /// </summary>
        [SheetColumn("Teléfono interno(内线电话)")]
        public string InnerTelePhone { get; set; }

        /// <summary>
        /// 部门等级
        /// </summary>
        [SheetColumn("Nivel administrativo(部门等级)")]
        public string Level { get; set; }

        /// <summary>
        /// 区号
        /// </summary>
        [SheetColumn("Codigo de área(区号)")]
        public string AreaCode { get; set; }

        /// <summary>
        /// 邮政编号
        /// </summary>
        [SheetColumn("Código postal(邮政编码)")]
        public string PostCode { get; set; }

        /// <summary>
        /// 服务类型
        /// </summary>
        [SheetColumn("Tipo de servicio(服务类型)")]
        public string ServiceType { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        [SheetColumn("Longitud(经度)")]
        public double? Longitude { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        [SheetColumn("Latitud(维度)")]
        public double? Latitude { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        [SheetColumn("Jefe(负责人)")]
        public string Boss { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [SheetColumn("Nota(备注)")]
        public string Note { get; set; }

        /// <summary>
        /// 忽略该列
        /// </summary>
        [Ignore]
        public string Ignore { get; set; }

        [RowNum]
        public int RowNum { get; set; }
    }

    /// <summary>
    /// 部门人员
    /// </summary>
    [Sheet("Personal(部门人员)")]
    internal class MockPerson
    {
        /// <summary>
        /// 人员名称
        /// </summary>
        [SheetColumn("Nombre(人员名称)")]
        public string Name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [SheetColumn("Género(性别)")]
        [SheetColumnValueConverter(typeof(GenderColumnValueConverter))]
        public Gender Gender { get; set; }

        /// <summary>
        /// 身份证
        /// </summary>
        [SheetColumn("Número ID(身份证号)")]
        public string IdCard { get; set; }

        [RowNum]
        public int RowNum { get; set; }
    }

    /// <summary>
    /// 行政区域
    /// </summary>
    [Sheet("行政区域")]
    internal class MockAdministrativeRegion
    {
        // ReSharper disable once InconsistentNaming
        public string DISTRICTID { get; set; }

        // ReSharper disable once InconsistentNaming
        public string PARENTID { get; set; }

        // ReSharper disable once InconsistentNaming
        public string DISTRICTFULLNAME { get; set; }

        [RowNum]
        public int RowNum { get; set; }
    }

    internal class GenderColumnValueConverter : SimpleValueConverter<string, Gender>
    {
        public override Gender ConvertFun(string source)
        {
            switch (source)
            {
                case "Hombre":
                    return Gender.Male;
                case "Mujer":
                    return Gender.Female;
                case "Transgénero":
                    return Gender.Neutral;
            }

            return Gender.None;
        }
    }

    internal enum Gender
    {
        /// <summary>
        /// 无效值
        /// </summary>
        None,

        /// <summary>
        /// 男性
        /// </summary>
        Male,

        /// <summary>
        /// 女性
        /// </summary>
        Female,

        /// <summary>
        /// 中性
        /// </summary>
        Neutral
    }
}
