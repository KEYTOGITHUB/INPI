using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using INPI.PM.Domain.ProjectService;
using INPI.PM.Domain.CustomerService;
using System.IO;
using System.Text;

namespace INPI.PM.AdminWeb.Controllers
{
    public class HomeController : Controller
    {
        private ProjectFactory _projectFactory = new ProjectFactory();

        public ActionResult Index()
        {
            var projects = _projectFactory.GetAllProjects();
            ViewBag.totalCount = projects == null ? 0 : projects.Count;
            ViewBag.finishCount = projects == null ? 0 : projects.Where(p => p.CurrentStep != null && p.CurrentStep.StepOrder == (int)RouteStep.发货).Count();
            ViewBag.designCount = projects == null ? 0 : projects.Where(p => p.CurrentStep != null && p.CurrentStep.StepOrder == (int)RouteStep.设计).Count();
            ViewBag.productCount = projects == null ? 0 : projects.Where(p => p.CurrentStep != null && p.CurrentStep.StepOrder == (int)RouteStep.生产).Count();
            ViewBag.housingCount = projects == null ? 0 : projects.Where(p => p.CurrentStep != null && p.CurrentStep.StepOrder == (int)RouteStep.入库).Count();
            return View();
        }

        public ActionResult ProjectList(int key=1)
        {
            ViewBag.key = key;
            ViewBag.ListName = "";
            ViewBag.projectList = new List<Project>();
            var projects = _projectFactory.GetAllProjects();
            if (projects != null)
            {
                switch (key)
                {
                    case 1:
                        ViewBag.projectList = projects;
                        ViewBag.ListName = "所有项目";
                        break;
                    case 2:
                        ViewBag.projectList = projects.Where(p => p.CurrentStep != null && p.CurrentStep.StepOrder > (int)RouteStep.发货).ToList();
                        ViewBag.ListName = "已完成项目";
                        break;
                    case 3:
                        ViewBag.projectList = projects.Where(p => p.CurrentStep == null || p.CurrentStep.StepOrder <= (int)RouteStep.发货).ToList();
                        ViewBag.ListName = "未完成项目";
                        break;
                    case 4:
                        ViewBag.projectList = projects.Where(p => p.CurrentStep != null && p.CurrentStep.StepOrder == (int)RouteStep.设计).ToList();
                        ViewBag.ListName = "已设计项目";
                        break;
                    case 5:
                        ViewBag.projectList = projects.Where(p => p.CurrentStep != null && p.CurrentStep.StepOrder == (int)RouteStep.生产).ToList();
                        ViewBag.ListName = "已生产项目";
                        break;
                    case 6:
                        ViewBag.projectList = projects.Where(p => p.CurrentStep != null && p.CurrentStep.StepOrder == (int)RouteStep.入库).ToList();
                        ViewBag.ListName = "已入库项目";
                        break;
                }
            }
            return View();
        }

        public ActionResult CustomerHome()
        {
            Contact currentCustomer = (Contact)Session["currentUser"];
            ViewBag.projects = _projectFactory.GetProjectsByContact(currentCustomer.GUID).OrderByDescending(p => p.EstimatedDeliveryDate);
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        #region 导出Excel
        public void ExportProject(int key)
        {
            var projectList = new List<Project>();
            var name = "";
            var projects = _projectFactory.GetAllProjects();
            if (projects != null)
            {
                switch (key)
                {
                    case 1:
                        projectList = projects;
                        name = "所有项目";
                        break;
                    case 2:
                        projectList = projects.Where(p => p.CurrentStep != null && p.CurrentStep.StepOrder > (int)RouteStep.发货).ToList();
                        name = "已完成项目";
                        break;
                    case 3:
                        projectList = projects.Where(p => p.CurrentStep == null || p.CurrentStep.StepOrder <= (int)RouteStep.发货).ToList();
                        name = "未完成项目";
                        break;
                    case 4:
                        projectList = projects.Where(p => p.CurrentStep != null && p.CurrentStep.StepOrder == (int)RouteStep.设计).ToList();
                        name = "已设计项目";
                        break;
                    case 5:
                        projectList = projects.Where(p => p.CurrentStep != null && p.CurrentStep.StepOrder == (int)RouteStep.生产).ToList();
                        name = "已生产项目";
                        break;
                    case 6:
                        projectList = projects.Where(p => p.CurrentStep != null && p.CurrentStep.StepOrder == (int)RouteStep.入库).ToList();
                        name = "已入库项目";
                        break;
                }
            }
            var tablename = string.Format("{0}_{1}_Excel", name, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
            var dt = new System.Data.DataTable();
            dt.TableName = tablename;
            dt.Columns.Add("序号");
            dt.Columns.Add("项目编号");
            dt.Columns.Add("项目名称");
            dt.Columns.Add("所属客户");
            dt.Columns.Add("预计交付日期");
            dt.Columns.Add("项目进度");
            dt.Columns.Add("预计完成时间");
            dt.Columns.Add("剩余天数");
            var i = 1;
            var customer = new CustomerFactory();
            foreach (var one in projectList)
            {
                dt.Rows.Add(
                    i.ToString(),
                    one.ProjectCode,
                    one.ProjectName,
                    string.IsNullOrEmpty(one.CustomerGuid) ? " " : customer.GetCustomer(one.CustomerGuid).CompanyName,
                    one.EstimatedDeliveryDate == null ? "" : one.EstimatedDeliveryDate.ToString("yyyy-MM-dd"),
                    one.CurrentStep == null ? "" : one.CurrentStep.StepName,
                    one.ProjectCurrentStepEstimatedCompleteDate == null ? "" : one.ProjectCurrentStepEstimatedCompleteDate.Value.ToString("yyyy-MM-dd"),
                    one.ProjectCurrentStepOverduDays
                    );
            }
            ExportExcel(dt);
        }

        public void ExportExcel(System.Data.DataTable dt)
        {
            try
            {
                var workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet(dt.TableName);
                IRow rowH = sheet.CreateRow(0);
                ICell cell = null;
                ICellStyle cellStyle = workbook.CreateCellStyle();
                IDataFormat dataFormat = workbook.CreateDataFormat();
                cellStyle.DataFormat = dataFormat.GetFormat("@");
                foreach (System.Data.DataColumn col in dt.Columns)
                {
                    rowH.CreateCell(col.Ordinal).SetCellValue(col.Caption);
                    rowH.Cells[col.Ordinal].CellStyle = cellStyle;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    IRow row = sheet.CreateRow(i + 1);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        cell = row.CreateCell(j);
                        cell.SetCellValue(dt.Rows[i][j].ToString());
                        cell.CellStyle = cellStyle;
                    }
                }
                var path = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/ExportExcels";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var savePath = path + "/" + dt.TableName + ".xls";
                if (System.IO.File.Exists(savePath))
                {
                    System.IO.File.Delete(savePath);
                }
                FileStream file = new FileStream(savePath, FileMode.CreateNew, FileAccess.Write);
                MemoryStream ms = new MemoryStream();
                workbook.Write(ms);
                byte[] bytes = ms.ToArray();
                file.Write(bytes, 0, bytes.Length);
                file.Flush();
                OutputClient(bytes);
                bytes = null;
                ms.Close();
                ms.Dispose();
                file.Close();
                file.Dispose();
                workbook.Close();
                sheet = null;
                workbook = null;
            }
            catch (Exception ex)
            {

            }
        }

        public void OutputClient(byte[] bytes)
        {
            HttpResponse response = System.Web.HttpContext.Current.Response;
            response.Buffer = true;
            response.Clear();
            response.ClearHeaders();
            response.ClearContent();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")));
            response.Charset = "GB2312";
            response.ContentEncoding = Encoding.GetEncoding("GB2312");
            response.BinaryWrite(bytes);
            response.Flush();
            response.Close();
        } 
        #endregion
    }
}