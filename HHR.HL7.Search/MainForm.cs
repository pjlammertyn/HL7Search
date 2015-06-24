using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using AsyncPoco;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Dynamic;
using Newtonsoft.Json.Converters;

namespace HHR.HL7.Search
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        #region Fields

        static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        IDictionary<string, Func<dynamic, Task<IList<dynamic>>>> patientByIdQueries = null;
        IDictionary<string, Func<dynamic, Task<IList<dynamic>>>> visitsByPatientIdQueries = null;
        IDictionary<string, Func<string, string, Task<IList<dynamic>>>> patientsByNameQueries = null;
        IDictionary<string, Func<dynamic, Task<IList<dynamic>>>> visitsByNumbersQueries = null;
        IDictionary<string, Func<dynamic, Task<IList<dynamic>>>> patientsByVisitNumbersQueries = null;
        IDictionary<string, Func<dynamic, Task<IList<dynamic>>>> visitDetailsByVisitNumbersQuery = null;
        IDictionary<string, Func<dynamic, Task<IList<dynamic>>>> doctorsByNumbersQuery = null;
        IDictionary<string, Func<dynamic, Task<IList<dynamic>>>> bedOccupationByWardIdsQuery = null;
        IDictionary<string, Func<dynamic, Task<IList<dynamic>>>> searchAdtQuery = null;
        IDictionary<string, Func<dynamic, Task<IList<dynamic>>>> searchMfnQuery = null;

        IDictionary<string, PropertyWindow> propertyWindows = null;
        IDictionary<string, ListWindow> listWindows = null;

        IDictionary<string, HL7TreeWindow> treeWindows = null;
        IDictionary<string, SyntaxHighlightingWindow> syntaxHighlightingWindows = null;

        SearchWindow m_searchWindow = null;
        SearchHL7ADTWindow m_searchHL7ADTWindow = null;
        SearchHL7MFNWindow m_searchHL7MFNWindow = null;

        OutputWindow m_outputWindow = null;
        ExportHL7Form m_exportHL7Form = null;

        WeifenLuo.WinFormsUI.Docking.DeserializeDockContent m_deserializeDockContent;

        System.Windows.Forms.MainMenu mainMenu;
        System.Windows.Forms.ImageList imageList;
        System.Windows.Forms.ToolBar toolBar;
        System.Windows.Forms.ToolBarButton toolBarButtonSearch;
        WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
        System.Windows.Forms.MenuItem menuItemView;
        System.Windows.Forms.MenuItem menuItemTools;
        System.Windows.Forms.MenuItem menuItemWindow;
        System.Windows.Forms.MenuItem menuItemHelp;
        System.Windows.Forms.StatusBar statusBar;
        System.Windows.Forms.MenuItem menuItem1;
        System.Windows.Forms.MenuItem menuItemToolbar;
        System.Windows.Forms.MenuItem menuItemStatusbar;
        System.Windows.Forms.MenuItem menuItemAbout;
        System.Windows.Forms.MenuItem menuItemLockLayout;
        System.Windows.Forms.MenuItem menuItem10;
        System.Windows.Forms.MenuItem menuItem2;
        System.Windows.Forms.MenuItem menuItemOutput;
        System.ComponentModel.IContainer components;
        System.Windows.Forms.MenuItem menuItemSearchByPatientIdOrVisitNumberWindow;
        System.Windows.Forms.MenuItem menuItemSearchHL7ADT;
        System.Windows.Forms.StatusBarPanel statusBarPanel;
        System.Windows.Forms.MenuItem menuItemSearchHL7MFN;
        System.Windows.Forms.MenuItem menuItemExportHL7;
        MenuItem menuItem8;
        MenuItem menuItemDefaultLayout;
        MenuItem menuItem11;
        System.Windows.Forms.MenuItem menuItem9;

        string biztalk;
        bool showOnlyLast100BiztalkHL7s;

        readonly string[] dbs = new string[] { "Oazis", "ZISv21", "ZISv23", "XdeCache", "Glims", "Glims Test", "Cyberlab", "Cyberlab Test", "Agfa", "Agfa Test",
				"Infohos Prs", "Infohos Prs Test", "Infohos CPD", "Aexis", "Acertis", "Infohos COZO"/*, "Cmeal", "Allgeier"*/};
        readonly string[] hl7Versions = new string[] { "HL7v21", "HL7v23", "Biztalk", "Biztalk Test" };

        #endregion

        #region Constructor

        public MainForm()
        {
            InitializeComponent();

            patientByIdQueries = new Dictionary<string, Func<dynamic, Task<IList<dynamic>>>>();
            visitsByPatientIdQueries = new Dictionary<string, Func<dynamic, Task<IList<dynamic>>>>();
            patientsByNameQueries = new Dictionary<string, Func<string, string, Task<IList<dynamic>>>>();
            visitsByNumbersQueries = new Dictionary<string, Func<dynamic, Task<IList<dynamic>>>>();
            patientsByVisitNumbersQueries = new Dictionary<string, Func<dynamic, Task<IList<dynamic>>>>();
            visitDetailsByVisitNumbersQuery = new Dictionary<string, Func<dynamic, Task<IList<dynamic>>>>();
            doctorsByNumbersQuery = new Dictionary<string, Func<dynamic, Task<IList<dynamic>>>>();
            bedOccupationByWardIdsQuery = new Dictionary<string, Func<dynamic, Task<IList<dynamic>>>>();
            searchAdtQuery = new Dictionary<string, Func<dynamic, Task<IList<dynamic>>>>();
            searchMfnQuery = new Dictionary<string, Func<dynamic, Task<IList<dynamic>>>>();

            propertyWindows = new Dictionary<string, PropertyWindow>();
            listWindows = new Dictionary<string, ListWindow>();

            treeWindows = new Dictionary<string, HL7TreeWindow>();
            syntaxHighlightingWindows = new Dictionary<string, SyntaxHighlightingWindow>();

            MenuItem menuItem;
            int menuItemViewIndex = 4;

            // get all sections inside group
            foreach (var key in dbs)
            {
                switch (key)
                {
                    case "Oazis":
                        patientByIdQueries.Add(key, async (id) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = @";
with mergedpatients (pat_id, pat_id_old, level)
as
(
	select u.pat_id, null, 0
	from oazp..unisuper u with (nolock)
	where pat_id = @0
	union all
	select auo.pat_id, auo.pat_id_old, 1
	from oazp..adt_unipat_old auo  with (nolock)
	where auo.pat_id_old = @0 or auo.pat_id = @0
	union all
	select auo.pat_id, auo.pat_id_old, level + 1
	from oazp..adt_unipat_old auo with (nolock)
	inner join mergedpatients m on m.pat_id = auo.pat_id_old
)
select au.*, u.pat_id_mother
from oazp..unisuper u with (nolock)
inner join oazp..adt_unipat au with (nolock) on u.pat_id = au.pat_id
where from_date = (select max(from_date) from oazp..adt_unipat with (nolock) where pat_id = u.pat_id)
and (u.pat_id = (select top 1 pat_id from mergedpatients with (nolock) order by level desc))";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, id.PadLeft(10, '0'));
                            }
                        });
                        visitsByPatientIdQueries.Add(key, async (id) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = @"select *
from oazp..adt_visit with (nolock)
where pat_id = @0
order by adm_date, adm_time";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, id.PadLeft(10, '0'));
                            }
                        });
                        patientsByNameQueries.Add(key, async (lastName, firstName) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = @"select top 100 au.*, u.pat_id_mother
from oazp..unisuper u with (nolock)
inner join oazp..adt_unipat au with (nolock) on u.pat_id = au.pat_id
where from_date = (select max(from_date) from oazp..adt_unipat with (nolock) where pat_id = u.pat_id)
and lastname like @0 and firstname like @1
order by lastname, firstname";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, lastName + "%", firstName + "%");
                            }
                        });
                        visitsByNumbersQueries.Add(key, async (numbers) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = @"select *
from oazp..adt_visit with (nolock)
where visit_id in (@0)
order by adm_date, adm_time";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, (from number in numbers as IEnumerable<string> select number.PadLeft(8, '0')));
                            }
                        });
                        patientsByVisitNumbersQueries.Add(key, async (numbers) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = @"select au.*, u.pat_id_mother
from oazp..unisuper u with (nolock)
inner join oazp..adt_unipat au with (nolock) on u.pat_id = au.pat_id
where from_date = (select max(from_date) from oazp..adt_unipat with (nolock) where pat_id = u.pat_id)
and u.pat_id in 
(
	select v.pat_id
	from oazp..adt_visit v with (nolock)
	where v.visit_id in (@0)
)";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, (from number in numbers as IEnumerable<string> select number.PadLeft(8, '0')));
                            }
                        });
                        visitDetailsByVisitNumbersQuery.Add(key, async (numbers) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = @"select vh.*
from oazp..adt_visit_hist vh with (nolock)
where vh.visit_id in (@0)
order by visit_id, from_date, from_time";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, (from number in numbers as IEnumerable<string> select number.PadLeft(8, '0')));
                            }
                        });
                        doctorsByNumbersQuery.Add(key, async (numbers) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = @";
with mergeddoctors (dokid, dokid_old, level)
as
(
	select dokid, dokid_old, 0
	from oazp..dokid with (nolock)
	where dokid in (@0) or dokid_old in (@0)
	union all
	select d.dokid, d.dokid_old, level + 1
	from oazp..dokid d with (nolock)
	inner join mergeddoctors m on m.dokid = d.dokid_old    
)
select d.*
from oazp..dokid di with (nolock)
inner join oazp..doctors d with (nolock) on d.uni_pers_no = di.dokid
where from_date = (select max(from_date) from oazp..doctors with (nolock) where uni_pers_no = d.uni_pers_no)
and (d.uni_pers_no = (select top 1 dokid from mergeddoctors with (nolock) order by level desc))";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, (from number in numbers as IEnumerable<string> select number.PadLeft(6, '0')));
                            }
                        });
                        bedOccupationByWardIdsQuery.Add(key, async (ids) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = @"select au.lastname, au.firstname, bo.*
from oazp..bedocc bo with (nolock)
inner join oazp..adt_unipat au with (nolock) on bo.pat_id = au.pat_id and au.from_date = (select max(from_date) from oazp..adt_unipat with (nolock) where pat_id = bo.pat_id)
where bo.ward_id in (@0) and bo.visit_id is not null
order by bo.room_id, bo.bed_id";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, (from id in ids as IEnumerable<string> select id.PadLeft(4, '0')));
                            }
                        });
                        break;
                    case "ZISv21":
                        patientByIdQueries.Add(key, async (id) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = @";
with mergedpatients (id, mergedpatientid, level)
as
(
	select p.id, p.mergedpatientid, 0 as level 
	from zisv21..patient p with (nolock)
	where p.id = @0 or p.mergedpatientid = @0
	union all
	select p.id, p.mergedpatientid, level + 1
	from zisv21..patient p with (nolock)
	inner join mergedpatients m on m.mergedpatientid = p.id
	where p.id <> 0
)
select * 
from zisv21..patient with (nolock)
where id = (select top 1 id from mergedpatients with (nolock) order by level desc)";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, id);
                            }
                        });
                        visitsByPatientIdQueries.Add(key, async (id) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = @"select *
from zisv21..visit with (nolock)
where patientid = @0";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, id);
                            }
                        });
                        patientsByNameQueries.Add(key, async (lastName, firstName) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = @"select top 100 * 
from zisv21..patient with (nolock)
where name like @0 and firstname like @1
order by name, firstname";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, lastName + "%", firstName + "%");
                            }
                        });
                        visitsByNumbersQueries.Add(key, async (numbers) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = @"select *
from zisv21..visit with (nolock)
where number in (@0)
order by admissiondate";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, numbers);
                            }
                        });
                        patientsByVisitNumbersQueries.Add(key, async (numbers) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = @"select * 
from zisv21..patient with (nolock)
where id in
(
	select patientid
	from zisv21..visit with (nolock)
	where number in (@0)
)";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, numbers);
                            }
                        });
                        visitDetailsByVisitNumbersQuery.Add(key, async (numbers) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = @"select *
from zisv21..visitdetail with (nolock)
where visitnumber in (@0)
order by visitnumber, fromdate";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, numbers);
                            }
                        });
                        doctorsByNumbersQuery.Add(key, async (numbers) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = @"select *
from zisv21..doctor with (nolock)
where number in (@0)";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, numbers);
                            }
                        });
                        break;
                    case "ZISv23":
                        patientByIdQueries.Add(key, async (id) =>
                        {
                            var connectionString = ConfigurationManager.ConnectionStrings[key].ConnectionString;
                            var requestUri = string.Format(@"zis_patient/_all_docs?include_docs=true&key=""{0}""", id.PadLeft(10, '0'));
                            Console.WriteLine("{0}: {1}", key, requestUri);
                            return await HttpGet(connectionString, requestUri).ConfigureAwait(false);
                        });
                        visitsByPatientIdQueries.Add(key, async (id) =>
                        {
                            var connectionString = ConfigurationManager.ConnectionStrings[key].ConnectionString;
                            var requestUri = string.Format(@"zis_visit/_design/docs/_view/by_patientid?include_docs=true&key=""{0}""", id.PadLeft(8, '0'));
                            Console.WriteLine("{0}: {1}", key, requestUri);
                            return await HttpGet(connectionString, requestUri).ConfigureAwait(false);
                        });
                        visitsByNumbersQueries.Add(key, async (numbers) =>
                        {
                            var connectionString = ConfigurationManager.ConnectionStrings[key].ConnectionString;
                            var requestUri = string.Format(@"zis_visit/_all_docs?include_docs=true&keys=[{0}]", string.Join(",", (from number in numbers as IEnumerable<string> select string.Concat("\"", number.PadLeft(8, '0'), "\""))));
                            Console.WriteLine("{0}: {1}", key, requestUri);
                            return await HttpGet(connectionString, requestUri).ConfigureAwait(false);
                        });
                        patientsByVisitNumbersQueries.Add(key, async (numbers) =>
                        {
                            var connectionString = ConfigurationManager.ConnectionStrings[key].ConnectionString;
                            var requestUri = string.Format(@"zis_visit/_all_docs?include_docs=true&keys=[{0}]", string.Join(",", (from number in numbers as IEnumerable<string> select string.Concat("\"", number.PadLeft(8, '0'), "\""))));
                            Console.WriteLine("{0}: {1}", key, requestUri);
                            using (var client = CreateHttpClient(new Uri(connectionString)))
                            {
                                var response = await client.GetAsync(requestUri).ConfigureAwait(false);
                                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                                //var task = Task.Run(() =>
                                //{
                                var result = JsonConvert.DeserializeObject<ExpandoObject>(content, new ExpandoObjectConverter());

                                if (((dynamic)result).rows.Count == 0)
                                    return null;

                                var patientIds = /*return*/ (from row in ((dynamic)result).rows as IEnumerable<dynamic>
                                                             where ((IDictionary<string, Object>)row).ContainsKey("doc") && (IDictionary<string, Object>)row.doc != null && ((IDictionary<string, Object>)row.doc).ContainsKey("PatientId")
                                                             select row.doc.PatientId as string).ToList();
                                //});
                                //await task.ConfigureAwait(false);

                                requestUri = string.Format(@"zis_patient/_all_docs?include_docs=true&keys=[{0}]", string.Join(",", (from number in patientIds select string.Concat("\"", number.PadLeft(10, '0'), "\""))));
                                Console.WriteLine("{0}: {1}", key, requestUri);
                                return await HttpGet(connectionString, requestUri).ConfigureAwait(false);
                            }
                        });
                        visitDetailsByVisitNumbersQuery.Add(key, async (numbers) =>
                        {
                            var connectionString = ConfigurationManager.ConnectionStrings[key].ConnectionString;
                            var requestUri = string.Format(@"zis_visit/_all_docs?include_docs=true&keys=[{0}]", string.Join(",", (from number in numbers as IEnumerable<string> select string.Concat("\"", number.PadLeft(8, '0'), "\""))));
                            Console.WriteLine("{0}: {1}", key, requestUri);
                            using (var client = CreateHttpClient(new Uri(connectionString)))
                            {
                                var response = await client.GetAsync(requestUri).ConfigureAwait(false);
                                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                                //return await Task.Run(() =>
                                //{
                                var result = JsonConvert.DeserializeObject<ExpandoObject>(content, new ExpandoObjectConverter());

                                if (((dynamic)result).rows.Count == 0)
                                    return null;

                                return (from row in ((dynamic)result).rows as IEnumerable<dynamic>
                                        where ((IDictionary<string, Object>)row).ContainsKey("doc") && (IDictionary<string, Object>)row.doc != null && ((IDictionary<string, Object>)row.doc).ContainsKey("Transfers")
                                        from transfer in ((IDictionary<string, Object>)row.doc)["Transfers"] as IEnumerable<dynamic>
                                        orderby row.doc.Id, transfer.FromDate
                                        select transfer).ToList();
                                //}).ConfigureAwait(false);
                            }
                        });
                        doctorsByNumbersQuery.Add(key, async (numbers) =>
                        {
                            var connectionString = ConfigurationManager.ConnectionStrings[key].ConnectionString;
                            var requestUri = string.Format(@"zis_doctor/_all_docs?include_docs=true&keys=[{0}]", string.Join(",", (from number in numbers as IEnumerable<string> select string.Concat("\"", number.PadLeft(6, '0'), "\""))));
                            Console.WriteLine("{0}: {1}", key, requestUri);
                            return await HttpGet(connectionString, requestUri).ConfigureAwait(false);
                        });
                        bedOccupationByWardIdsQuery.Add(key, async (ids) =>
                        {
                            var connectionString = ConfigurationManager.ConnectionStrings[key].ConnectionString;
                            var requestUri = string.Format(@"zis_ward/_design/docs/_view/by_id?include_docs=true&keys=[{0}]", string.Join(",", (from id in ids as IEnumerable<string> select string.Concat("\"", id.PadLeft(4, '0'), "\""))));
                            Console.WriteLine("{0}: {1}", key, requestUri);
                            using (var client = CreateHttpClient(new Uri(connectionString)))
                            {
                                var response = await client.GetAsync(requestUri).ConfigureAwait(false);
                                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                                //return await Task.Run(() =>
                                //{
                                var result = JsonConvert.DeserializeObject<ExpandoObject>(content, new ExpandoObjectConverter());

                                if (((dynamic)result).rows.Count == 0)
                                    return null;

                                return (from row in ((dynamic)result).rows as IEnumerable<dynamic>
                                        where ((IDictionary<string, Object>)row).ContainsKey("doc") && (IDictionary<string, Object>)row.doc != null && ((IDictionary<string, Object>)row.doc).ContainsKey("Beds")
                                        from bed in ((IDictionary<string, Object>)row.doc)["Beds"] as IEnumerable<dynamic>
                                        let location = string.Concat((bed as IDictionary<string, Object>).ContainsKey("RoomId") ? bed.RoomId : string.Empty, "-", (bed as IDictionary<string, Object>).ContainsKey("BedId") ? bed.BedId : string.Empty)
                                        orderby location
                                        select bed).ToList();
                                //}).ConfigureAwait(false);
                            }
                        });
                        break;
                    case "XdeCache":
                        patientByIdQueries.Add(key, async (id) =>
                        {
                            var connectionString = ConfigurationManager.ConnectionStrings[key].ConnectionString;
                            var requestUri = string.Format(@"patient/_design/docs/_view/by_id?include_docs=true&key=""{0}""", id.PadLeft(10, '0'));
                            Console.WriteLine("{0}: {1}", key, requestUri);
                            using (var client = CreateHttpClient(new Uri(connectionString)))
                            {
                                var response = await client.GetAsync(requestUri).ConfigureAwait(false);
                                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                                //return await Task.Run(async () =>
                                //{
                                var result = JsonConvert.DeserializeObject<ExpandoObject>(content, new ExpandoObjectConverter());

                                var mergedPatid = (from row in ((dynamic)result).rows as IEnumerable<dynamic>
                                                   where row.doc.PAT_ID != id
                                                   select row.doc.PAT_ID as string).FirstOrDefault();
                                if (!mergedPatid.IsNullOrEmpty())
                                    return await patientByIdQueries[key](mergedPatid);

                                if (((dynamic)result).rows.Count == 0)
                                    return null;

                                return (from row in ((dynamic)result).rows as IEnumerable<dynamic>
                                        where ((IDictionary<string, Object>)row).ContainsKey("doc")
                                        select row.doc).ToList();
                                //}).ConfigureAwait(false);
                            }
                        });
                        visitsByPatientIdQueries.Add(key, async (id) =>
                        {
                            var connectionString = ConfigurationManager.ConnectionStrings[key].ConnectionString;
                            var requestUri = string.Format(@"visit/_design/docs/_view/by_patid?include_docs=true&key=""{0}""", id.PadLeft(8, '0'));
                            Console.WriteLine("{0}: {1}", key, requestUri);
                            return await HttpGet(connectionString, requestUri).ConfigureAwait(false);
                        });
                        visitsByNumbersQueries.Add(key, async (numbers) =>
                        {
                            var connectionString = ConfigurationManager.ConnectionStrings[key].ConnectionString;
                            var requestUri = string.Format(@"visit/_all_docs?include_docs=true&keys=[{0}]", string.Join(",", (from number in numbers as IEnumerable<string> select string.Concat("\"", number.PadLeft(8, '0'), "\""))));
                            Console.WriteLine("{0}: {1}", key, requestUri);
                            return await HttpGet(connectionString, requestUri).ConfigureAwait(false);
                        });
                        patientsByVisitNumbersQueries.Add(key, async (numbers) =>
                        {
                            var connectionString = ConfigurationManager.ConnectionStrings[key].ConnectionString;
                            var requestUri = string.Format(@"visit/_all_docs?include_docs=true&keys=[{0}]", string.Join(",", (from number in numbers as IEnumerable<string> select string.Concat("\"", number.PadLeft(8, '0'), "\""))));
                            Console.WriteLine("{0}: {1}", key, requestUri);
                            using (var client = CreateHttpClient(new Uri(connectionString)))
                            {
                                var response = await client.GetAsync(requestUri).ConfigureAwait(false);
                                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                                //var task = Task.Run(() =>
                                //{
                                var result = JsonConvert.DeserializeObject<ExpandoObject>(content, new ExpandoObjectConverter());

                                if (((dynamic)result).rows.Count == 0)
                                    return null;

                                var patientIds = (from row in ((dynamic)result).rows as IEnumerable<dynamic>
                                                  where ((IDictionary<string, Object>)row).ContainsKey("doc") && ((IDictionary<string, Object>)row.doc).ContainsKey("PAT_ID")
                                                  select row.doc.PAT_ID as string).ToList();
                                //});
                                //await task.ConfigureAwait(false);

                                requestUri = string.Format(@"patient/_all_docs?include_docs=true&keys=[{0}]", string.Join(",", (from number in patientIds select string.Concat("\"", number.PadLeft(10, '0'), "\""))));
                                Console.WriteLine("{0}: {1}", key, requestUri);
                                return await HttpGet(connectionString, requestUri).ConfigureAwait(false);
                            }
                        });
                        visitDetailsByVisitNumbersQuery.Add(key, async (numbers) =>
                        {
                            var connectionString = ConfigurationManager.ConnectionStrings[key].ConnectionString;
                            var requestUri = string.Format(@"visit/_all_docs?include_docs=true&keys=[{0}]", string.Join(",", (from number in numbers as IEnumerable<string> select string.Concat("\"", number.PadLeft(8, '0'), "\""))));
                            Console.WriteLine("{0}: {1}", key, requestUri);
                            using (var client = CreateHttpClient(new Uri(connectionString)))
                            {
                                var response = await client.GetAsync(requestUri).ConfigureAwait(false);
                                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                                //return await Task.Run(() =>
                                //{
                                var result = JsonConvert.DeserializeObject<ExpandoObject>(content, new ExpandoObjectConverter());

                                if (((dynamic)result).rows.Count == 0)
                                    return null;

                                return (from row in ((dynamic)result).rows as IEnumerable<dynamic>
                                        where ((IDictionary<string, Object>)row).ContainsKey("doc") && ((IDictionary<string, Object>)row.doc).ContainsKey("Transfers")
                                        from transfer in ((IDictionary<string, Object>)row.doc)["Transfers"] as IEnumerable<dynamic>
                                        orderby transfer.VISIT_ID, transfer.FROM_DATE, transfer.FROM_TIME
                                        select transfer).ToList();
                                //}).ConfigureAwait(false);
                            }
                        });
                        doctorsByNumbersQuery.Add(key, async (numbers) =>
                        {
                            var connectionString = ConfigurationManager.ConnectionStrings[key].ConnectionString;
                            var requestUri = string.Format(@"doctor/_all_docs?include_docs=true&keys=[{0}]", string.Join(",", (from number in numbers as IEnumerable<string> select string.Concat("\"", number.PadLeft(6, '0'), "\""))));
                            Console.WriteLine("{0}: {1}", key, requestUri);
                            return await HttpGet(connectionString, requestUri).ConfigureAwait(false);
                        });
                        bedOccupationByWardIdsQuery.Add(key, async (ids) =>
                        {
                            var connectionString = ConfigurationManager.ConnectionStrings[key].ConnectionString;
                            var requestUri = string.Format(@"bedocc/_all_docs?include_docs=true&keys=[{0}]", string.Join(",", (from id in ids as IEnumerable<string> select string.Concat("\"", id.PadLeft(4, '0'), "\""))));
                            Console.WriteLine("{0}: {1}", key, requestUri);
                            using (var client = CreateHttpClient(new Uri(connectionString)))
                            {
                                var response = await client.GetAsync(requestUri).ConfigureAwait(false);
                                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                                //return await Task.Run(() =>
                                //{
                                var result = JsonConvert.DeserializeObject<ExpandoObject>(content, new ExpandoObjectConverter());

                                if (((dynamic)result).rows.Count == 0)
                                    return null;

                                return (from row in ((dynamic)result).rows as IEnumerable<dynamic>
                                        where ((IDictionary<string, Object>)row).ContainsKey("doc") && ((IDictionary<string, Object>)row.doc).ContainsKey("Beds")
                                        from bed in ((IDictionary<string, Object>)row.doc)["Beds"] as IEnumerable<dynamic>
                                        where !string.IsNullOrEmpty(bed.VISIT_ID)
                                        orderby bed.ROOM_ID, bed.BED_ID
                                        select bed).ToList();
                                //}).ConfigureAwait(false);
                            }
                        });
                        break;
                    case "Glims":
                    case "Glims Test":
                        patientByIdQueries.Add(key, async (id) =>
                        {
                            using (var db = new Database(key) { EnableNamedParams = false })
                            {
                                var sql = @"select *
from PUB.Identification
inner join PUB.Correspondent on crsp_Id = idnt_Target
inner join PUB.Person on prsn_Correspondent = crsp_Id
left outer join PUB.Municipality on mncp_Id = crsp_Municipality
left outer join PUB.Country on cnty_Id = mncp_Country
left outer join PUB.CountryName on ml68_Name = cnty_Id and ml68_Language = 3
left outer join PUB.HCProvider on hcpr_Id = prsn_FamilyDoctor
where idnt_Code = ?";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, id);
                            }
                        });
                        visitsByPatientIdQueries.Add(key, async (id) =>
                        {
                            using (var db = new Database(key) { EnableNamedParams = false })
                            {
                                var sql = @"select *
from PUB.Encounter
left outer join PUB.EncounterType on entp_Id = enct_Type
left outer join PUB.HCProvider on hcpr_Id = enct_Physician
left outer join PUB.Institution on inst_Id = enct_Institution
where enct_Person = 
(
   select prsn_Id
   from PUB.Identification
   inner join PUB.Correspondent on idnt_Target = crsp_Id
   inner join PUB.Person on prsn_Correspondent = crsp_Id
   where idnt_Code = ?
)";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, id);
                            }
                        });
                        patientsByNameQueries.Add(key, async (lastName, firstName) =>
                        {
                            using (var db = new Database(key) { EnableNamedParams = false })
                            {
                                var sql = @"select top 100 *
from PUB.Identification
inner join PUB.Correspondent on crsp_Id = idnt_Target
inner join PUB.Person on prsn_Correspondent = crsp_Id
left outer join PUB.Municipality on mncp_Id = crsp_Municipality
left outer join PUB.Country on cnty_Id = mncp_Country
left outer join PUB.CountryName on ml68_Name = cnty_Id and ml68_Language = 3
left outer join PUB.HCProvider on hcpr_Id = prsn_FamilyDoctor
where prsn_LastName like ? and prsn_FirstName like ?
order by prsn_LastName, prsn_FirstName";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                //return await db.FetchAsync<dynamic>(sql, lastName + "%", firstName + "%"); //NOT WORKING
                                sql = string.Format(@"select top 100 *
from PUB.Identification
inner join PUB.Correspondent on crsp_Id = idnt_Target
inner join PUB.Person on prsn_Correspondent = crsp_Id
left outer join PUB.Municipality on mncp_Id = crsp_Municipality
left outer join PUB.Country on cnty_Id = mncp_Country
left outer join PUB.CountryName on ml68_Name = cnty_Id and ml68_Language = 3
left outer join PUB.HCProvider on hcpr_Id = prsn_FamilyDoctor
where prsn_LastName like '{0}' and prsn_FirstName like '{1}'
order by prsn_LastName, prsn_FirstName", lastName + "%", firstName + "%");
                                return await db.FetchAsync<dynamic>(sql);
                            }
                        });
                        visitsByNumbersQueries.Add(key, async (numbers) =>
                        {
                            using (var db = new Database(key) { EnableNamedParams = false })
                            {
                                var sql = @"select *
from PUB.Encounter
left outer join PUB.EncounterType on entp_Id = enct_Type
left outer join PUB.HCProvider on hcpr_Id = enct_Physician
left outer join PUB.Institution on inst_Id = enct_Institution
where enct_ExternalId in (?)";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                //return await db.FetchAsync<dynamic>(sql, numbers); //NOT WORKING
                                sql = string.Format(@"select *
from PUB.Encounter
left outer join PUB.EncounterType on entp_Id = enct_Type
left outer join PUB.HCProvider on hcpr_Id = enct_Physician
left outer join PUB.Institution on inst_Id = enct_Institution
where enct_ExternalId in ({0})", string.Join(",", (from number in numbers as IEnumerable<string> select string.Concat("'", number.PadLeft(8, '0'), "'"))));
                                return await db.FetchAsync<dynamic>(sql);
                            }
                        });
                        patientsByVisitNumbersQueries.Add(key, async (numbers) =>
                        {
                            using (var db = new Database(key) { EnableNamedParams = false })
                            {
                                var sql = @"select *
from PUB.Identification
inner join PUB.Correspondent on idnt_Target = crsp_Id
inner join PUB.Person on prsn_Correspondent = crsp_Id
where prsn_Id in 
(
	select enct_Person
	from PUB.Encounter
	where enct_ExternalId in (?)
)";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                //return await db.FetchAsync<dynamic>(sql, numbers); //NOT WORKING
                                sql = string.Format(@"select enct_Person
from PUB.Encounter
where enct_ExternalId in ({0})", string.Join(",", (from number in numbers as IEnumerable<string> select string.Concat("'", number.PadLeft(8, '0'), "'"))));
                                var patientIds = await db.FetchAsync<int>(sql);
                                sql = string.Format(@"select *
from PUB.Identification
inner join PUB.Correspondent on idnt_Target = crsp_Id
inner join PUB.Person on prsn_Correspondent = crsp_Id
where prsn_Id in ({0})", string.Join(",", (from id in patientIds select id.ToString())));
                                return await db.FetchAsync<dynamic>(sql);
                            }
                        });
                        visitDetailsByVisitNumbersQuery.Add(key, async (numbers) =>
                        {
                            using (var db = new Database(key) { EnableNamedParams = false })
                            {
                                var sql = @"select *
from PUB.Stay
where stay_Encounter in 
(
	select enct_Id
	from PUB.Encounter
	where enct_ExternalId in (?)
)";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                //return await db.FetchAsync<dynamic>(sql, numbers); //NOT WORKING
                                sql = string.Format(@"select enct_Id
from PUB.Encounter
where enct_ExternalId in ({0})", string.Join(",", (from number in numbers as IEnumerable<string> select string.Concat("'", number.PadLeft(8, '0'), "'"))));
                                var visitIds = await db.FetchAsync<int>(sql);
                                if (!visitIds.Any())
                                    return Enumerable.Empty<dynamic>().ToList();
                                sql = string.Format(@"select *
from PUB.Stay
where stay_Encounter in ({0})", string.Join(",", (from id in visitIds select id.ToString())));
                                return await db.FetchAsync<dynamic>(sql);
                            }
                        });
                        doctorsByNumbersQuery.Add(key, async (numbers) =>
                        {
                            using (var db = new Database(key) { EnableNamedParams = false })
                            {
                                var sql = @"select *
from PUB.Encounter
left outer join PUB.EncounterType on entp_Id = enct_Type
left outer join PUB.HCProvider on hcpr_Id = enct_Physician
left outer join PUB.Institution on inst_Id = enct_Institution
where enct_ExternalId in (?)";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                //return await db.FetchAsync<dynamic>(sql, numbers); //NOT WORKING
                                sql = string.Format(@"select *
from PUB.HCProvider
left outer join PUB.Correspondent on crsp_Id = hcpr_Correspondent
left outer join PUB.Organization on org_Id = hcpr_Practice
where hcpr_Code in ({0})", string.Join(",", (from number in numbers as IEnumerable<string> select string.Concat("'", number.PadLeft(5, '0'), "'"))));
                                return await db.FetchAsync<dynamic>(sql);
                            }
                        });
                        break;
                    case "Agfa":
                    case "Agfa Test":
                        patientByIdQueries.Add(key, async (id) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = @"select p.*, d_hospitalkey
from patients p
inner join doctors d on d_key = p_physician
where p_code = @0";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, id);
                            }
                        });
                        visitsByPatientIdQueries.Add(key, async (id) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = @"select se.*, p_code
from service_episodes se
inner join patients p on p_key = se_p_key
where p_code = @0
order by se.se_admission_date_hospital";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, id.PadLeft(10, '0'));
                            }
                        });
                        patientsByNameQueries.Add(key, async (lastName, firstName) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = @"select p.*, d_hospitalkey
from patient p
inner join doctors d on d_key = p_physician
where lower(pat_lastname) like @0 and lower(pat_firstname) like @1
and rownum <= 100
order by pat_lastname, pat_firstname";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, lastName + "%", firstName + "%");
                            }
                        });
                        visitsByNumbersQueries.Add(key, async (numbers) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = @"select se.*, p_code
from service_episodes se
inner join patients p on p_key = se_p_key
where se_admission_number in (@0)
order by se_admission_date_hospital";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, numbers);
                            }
                        });
                        patientsByVisitNumbersQueries.Add(key, async (numbers) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = @"select p.*, d_hospitalkey
from patients p
inner join doctors d on d_key = p_physician
inner join service_episodes se on se_p_key = p_key
where se_admission_number in (@0)";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, numbers);
                            }
                        });
                        visitDetailsByVisitNumbersQuery.Add(key, async (numbers) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = @"select *
from transfers
where tr_admission_nr in (@0)
order by tr_admission_nr, tr_date_from";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, numbers);
                            }
                        });
                        doctorsByNumbersQuery.Add(key, async (numbers) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = @"select *
from doctors
left outer join services on s_key = d_service
left outer join hospitals on h_key = d_hospital
left outer join specialism on sp_key = d_specialism
where d_hospitalkey in (@0)";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, numbers);
                            }
                        });
                        break;
                    case "Cyberlab":
                    case "Cyberlab Test":
                        patientByIdQueries.Add(key, async (id) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = @"select *
from patient
left outer join account on acnt_patient = pat_id
where pat_code = @0";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, id);
                            }
                        });
                        visitsByPatientIdQueries.Add(key, async (id) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = @"select *
from visit 
inner join patient on pat_id = vst_patient
inner join ""GROUP"" on grp_id = vst_group
where pat_code = @0
order by vst_startdate";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, id.PadLeft(10, '0'));
                            }
                        });
                        patientsByNameQueries.Add(key, async (lastName, firstName) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = @"select *
from patient
left outer join account on acnt_patient = pat_id
where lower(pat_lastname) like @0 and lower(pat_firstname) like @1
and rownum <= 100
order by pat_lastname, pat_firstname";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, lastName + "%", firstName + "%");
                            }
                        });
                        visitsByNumbersQueries.Add(key, async (numbers) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = @"select *
from visit 
inner join ""GROUP"" on grp_id = vst_group
where vst_code in (@0)";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, (from number in numbers as IEnumerable<string> select number.PadLeft(8, '0')));
                            }
                        });
                        patientsByVisitNumbersQueries.Add(key, async (numbers) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = @"select patient.*, account.*
from patient 
left outer join account on acnt_patient = pat_id
inner join visit on vst_patient = pat_id
where vst_code in (@0)";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, (from number in numbers as IEnumerable<string> select number.PadLeft(8, '0')));
                            }
                        });
                        visitDetailsByVisitNumbersQuery.Add(key, async (numbers) =>
                        {
                            return await visitsByNumbersQueries[key](numbers);
                        });
                        doctorsByNumbersQuery.Add(key, async (numbers) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = @"select *
from issuer
where iss_code in (@0)";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, (from number in numbers as IEnumerable<string> select number.PadLeft(6, '0')));
                            }
                        });
                        break;
                    case "Infohos Prs":
                    case "Infohos Prs Test":
                        patientByIdQueries.Add(key, async (id) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = string.Format(@"select p.*, dr.nr_id as huisarts_id
from {0}..tab_pat p with (nolock)
left outer join {0}..tab_dokters dr with (nolock) on dr.nr_dok = p.huisarts
where p.nr_pat = @0", key == "Infohos Prs" ? "prs054" : "prs954");
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, id.PadLeft(10, '0'));
                            }
                        });
                        visitsByPatientIdQueries.Add(key, async (id) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = string.Format(@"select d.*, dr.nr_id as dok_verwezen_id
from {0}..dossier d with (nolock)
left outer join {0}..tab_dokters dr with (nolock) on dr.nr_dok = d.nr_dok_verwezen
where d.nr_pat = @0
order by d.dat_opn", key == "Infohos Prs" ? "prs054" : "prs954");
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                var visits = await db.FetchAsync<dynamic>(sql, id.PadLeft(10, '0'));

                                sql = string.Format(@"select vo.*, dr.nr_id as dok_verwezen_id
from {0}..vooropname vo with (nolock)
left outer join {0}..tab_dokters dr with (nolock) on dr.nr_dok = vo.nr_dok
where vo.nr_pat = @0
order by vo.dat_begin", key == "Infohos Prs" ? "prs054" : "prs954");
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                visits.AddRange(await db.FetchAsync<dynamic>(sql, id.PadLeft(10, '0')));
                                return visits;
                            }
                        });
                        patientsByNameQueries.Add(key, async (lastName, firstName) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = string.Format(@"select top 100 p.*, dr.nr_id as huisarts_id
from {0}..tab_pat p with (nolock)
left outer join {0}..tab_dokters dr with (nolock) on dr.nr_dok = p.huisarts
where p.fnaam_pat like @0 and p.vnaam_pat like @1
order by p.fnaam_pat, p.vnaam_pat", key == "Infohos Prs" ? "prs054" : "prs954");
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, lastName + "%", firstName + "%");
                            }
                        });
                        visitsByNumbersQueries.Add(key, async (numbers) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = string.Format(@"select d.*, dr.nr_id as dok_verwezen_id
from {0}..dossier d with (nolock)
left outer join {0}..tab_dokters dr with (nolock) on dr.nr_dok = d.nr_dok_verwezen
where d.nr_dos in (@0)
order by d.dat_opn", key == "Infohos Prs" ? "prs054" : "prs954");
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                var visits = await db.FetchAsync<dynamic>(sql, (from number in numbers as IEnumerable<string> select number.PadLeft(8, '0')));

                                sql = string.Format(@"select vo.*, dr.nr_id as dok_verwezen_id
from {0}..vooropname vo with (nolock)
left outer join {0}..tab_dokters dr with (nolock) on dr.nr_dok = vo.nr_dok
where vo.nr_vooropname in (@0)
order by vo.dat_begin", key == "Infohos Prs" ? "prs054" : "prs954");
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                visits.AddRange(await db.FetchAsync<dynamic>(sql, (from number in numbers as IEnumerable<string> select number.PadLeft(8, '0'))));
                                return visits;
                            }
                        });
                        patientsByVisitNumbersQueries.Add(key, async (numbers) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = string.Format(@"select p.*, dr.nr_id as huisarts_id
from {0}..tab_pat p with (nolock)
inner join {0}..tab_dokters dr with (nolock) on dr.nr_dok = p.huisarts
inner join {0}..dossier d on p.nr_pat = d.nr_pat
where d.nr_dos in (@0)", key == "Infohos Prs" ? "prs054" : "prs954");
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, (from number in numbers as IEnumerable<string> select number.PadLeft(8, '0')));
                            }
                        });
                        visitDetailsByVisitNumbersQuery.Add(key, async (numbers) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = string.Format(@"select vp.*--, v.veoms
from {0}..verper vp with (nolock)
--left outer join {1}..verpleeg v with (nolock) on v.venr = vp.nr_ve
where vp.nr_dos in (@0)
order by vp.nr_dos, vp.dat_begin", key == "Infohos Prs" ? "prs054" : "prs954", key == "Infohos Prs" ? "sql05.apot054" : "sql05.apot954");
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, (from number in numbers as IEnumerable<string> select number.PadLeft(8, '0')));
                            }
                        });
                        doctorsByNumbersQuery.Add(key, async (numbers) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = string.Format(@"select dr.*
from {0}..tab_dokters dr with (nolock)
where dr.nr_id in (@0)", key == "Infohos Prs" ? "prs054" : "prs954");
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, (from number in numbers as IEnumerable<string> select number.PadLeft(6, '0')));
                            }
                        });
                        break;
                    case "Infohos CPD":
                        patientByIdQueries.Add(key, async (id) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = @"select p.*, 
(
	select luv.Description
	from cpd_azdelta_prod..LUValue luv with (nolock)
	inner join cpd_azdelta_prod..lutable lut with (nolock) on lut.LUTableID = luv.LUTableID
	where lut.TableName = 'SEXES' and luv.LUValueID = p.SexId
) as SexDescription,
(
	select luv.Description
	from cpd_azdelta_prod..LUValue luv with (nolock)
	inner join cpd_azdelta_prod..lutable lut with (nolock) on lut.LUTableID = luv.LUTableID
	where lut.TableName = 'NATIONALITIES' and luv.LUValueID = p.NationalityID
) as NationalityDescription,
(
	select luv.Description
	from cpd_azdelta_prod..LUValue luv with (nolock)
	inner join cpd_azdelta_prod..lutable lut with (nolock) on lut.LUTableID = luv.LUTableID
	where lut.TableName = 'LANGUAGES' and luv.LUValueID = p.LanguageID
) as LanguageDescription,
dr.administrativeid as GeneralPractitionerAdministrativeId, dr.LastName as GeneralPractitionerLastName, dr.FirstName as GeneralPractitionerFirstName
from cpd_azdelta_prod..cpdpatient p with (nolock)
left outer join cpd_azdelta_prod..cpddoctor dr with (nolock) on dr.cpddoctorid = p.generalpractitionerid 
where p.patientnr = @0";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, id.PadLeft(10, '0'));
                            }
                        });
                        visitsByPatientIdQueries.Add(key, async (id) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = string.Format(@"select p.PatientNr, d.*, dr.DoctorNbr as TransmittingGpDoctorNbr, 
(
	select luv.Description
	from cpd_azdelta_prod..LUValue luv with (nolock)
	inner join cpd_azdelta_prod..lutable lut with (nolock) on lut.LUTableID = luv.LUTableID
	where lut.TableName = 'DOSSIERTYPES' and luv.LUValueID = d.DossierTypeID
) as DossierTypeDescription,
(
	select luv.Description
	from cpd_azdelta_prod..LUValue luv with (nolock)
	inner join cpd_azdelta_prod..lutable lut with (nolock) on lut.LUTableID = luv.LUTableID
	where lut.TableName = 'DOSSIERCDS' and luv.LUValueID = d.DossierCdID
) as DossierCodeDescription,
(
	select luv.Description
	from cpd_azdelta_prod..LUValue luv with (nolock)
	inner join cpd_azdelta_prod..lutable lut with (nolock) on lut.LUTableID = luv.LUTableID
	where lut.TableName = 'ACCIDENTCDS' and luv.LUValueID = d.AccidentCdID
) as AccidentDescription
from cpd_azdelta_prod..cpddossier d with (nolock)
inner join cpd_azdelta_prod..cpdpatient p with (nolock) on p.cpdpatientid = d.cpdpatientid
left outer join cpd_azdelta_prod..cpddoctor dr with (nolock) on dr.cpddoctorid = d.transmittinggpid 
where p.patientnr = @0
order by d.startdate");
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, id.PadLeft(10, '0'));
                            }
                        });
                        patientsByNameQueries.Add(key, async (lastName, firstName) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = string.Format(@"select top 100 p.*, 
(
	select luv.Description
	from cpd_azdelta_prod..LUValue luv with (nolock)
	inner join cpd_azdelta_prod..lutable lut with (nolock) on lut.LUTableID = luv.LUTableID
	where lut.TableName = 'SEXES' and luv.LUValueID = p.SexId
) as SexDescription,
(
	select luv.Description
	from cpd_azdelta_prod..LUValue luv with (nolock)
	inner join cpd_azdelta_prod..lutable lut with (nolock) on lut.LUTableID = luv.LUTableID
	where lut.TableName = 'NATIONALITIES' and luv.LUValueID = p.NationalityID
) as NationalityDescription,
(
	select luv.Description
	from cpd_azdelta_prod..LUValue luv with (nolock)
	inner join cpd_azdelta_prod..lutable lut with (nolock) on lut.LUTableID = luv.LUTableID
	where lut.TableName = 'LANGUAGES' and luv.LUValueID = p.LanguageID
) as LanguageDescription,
dr.administrativeid as GeneralPractitionerAdministrativeId, dr.LastName as GeneralPractitionerLastName, dr.FirstName as GeneralPractitionerFirstName
from cpd_azdelta_prod..cpdpatient p with (nolock)
left outer join cpd_azdelta_prod..cpddoctor dr with (nolock) on dr.cpddoctorid = p.generalpractitionerid 
where p.LastName like @0 and p.FirstName like @1
order by p.LastName, p.FirstName");
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, lastName + "%", firstName + "%");
                            }
                        });
                        visitsByNumbersQueries.Add(key, async (numbers) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = @"select p.PatientNr, d.*, dr.DoctorNbr as TransmittingGpDoctorNbr, 
(
	select luv.Description
	from cpd_azdelta_prod..LUValue luv with (nolock)
	inner join cpd_azdelta_prod..lutable lut with (nolock) on lut.LUTableID = luv.LUTableID
	where lut.TableName = 'DOSSIERTYPES' and luv.LUValueID = d.DossierTypeID
) as DossierTypeDescription,
(
	select luv.Description
	from cpd_azdelta_prod..LUValue luv with (nolock)
	inner join cpd_azdelta_prod..lutable lut with (nolock) on lut.LUTableID = luv.LUTableID
	where lut.TableName = 'DOSSIERCDS' and luv.LUValueID = d.DossierCdID
) as DossierCodeDescription,
(
	select luv.Description
	from cpd_azdelta_prod..LUValue luv with (nolock)
	inner join cpd_azdelta_prod..lutable lut with (nolock) on lut.LUTableID = luv.LUTableID
	where lut.TableName = 'ACCIDENTCDS' and luv.LUValueID = d.AccidentCdID
) as AccidentDescription
from cpd_azdelta_prod..cpddossier d with (nolock)
inner join cpd_azdelta_prod..cpdpatient p with (nolock) on p.cpdpatientid = d.cpdpatientid
left outer join cpd_azdelta_prod..cpddoctor dr with (nolock) on dr.cpddoctorid = d.transmittinggpid 
where d.dossiernr in (@0)
order by d.startdate";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, (from number in numbers as IEnumerable<string> select number.PadLeft(8, '0')));
                            }
                        });
                        patientsByVisitNumbersQueries.Add(key, async (numbers) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = string.Format(@"select top 100 p.*, 
(
	select luv.Description
	from cpd_azdelta_prod..LUValue luv with (nolock)
	inner join cpd_azdelta_prod..lutable lut with (nolock) on lut.LUTableID = luv.LUTableID
	where lut.TableName = 'SEXES' and luv.LUValueID = p.SexId
) as SexDescription,
(
	select luv.Description
	from cpd_azdelta_prod..LUValue luv with (nolock)
	inner join cpd_azdelta_prod..lutable lut with (nolock) on lut.LUTableID = luv.LUTableID
	where lut.TableName = 'NATIONALITIES' and luv.LUValueID = p.NationalityID
) as NationalityDescription,
(
	select luv.Description
	from cpd_azdelta_prod..LUValue luv with (nolock)
	inner join cpd_azdelta_prod..lutable lut with (nolock) on lut.LUTableID = luv.LUTableID
	where lut.TableName = 'LANGUAGES' and luv.LUValueID = p.LanguageID
) as LanguageDescription,
dr.administrativeid as GeneralPractitionerAdministrativeId, dr.LastName as GeneralPractitionerLastName, dr.FirstName as GeneralPractitionerFirstName
from cpd_azdelta_prod..cpdpatient p with (nolock)
inner join cpd_azdelta_prod..cpddossier d with (nolock) on d.cpdpatientid = p.cpdpatientId
left outer join cpd_azdelta_prod..cpddoctor dr with (nolock) on dr.cpddoctorid = p.generalpractitionerid 
where d.dossiernr in (@0)
order by p.LastName, p.FirstName");
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, (from number in numbers as IEnumerable<string> select number.PadLeft(8, '0')));
                            }
                        });
                        visitDetailsByVisitNumbersQuery.Add(key, async (numbers) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = string.Format(@"select p.PatientNr, d.dossiernr, c.campusnbr, c.campusdescription, cu.nbrcareunit, cu.description as careunitdescription, r.roomnbr, b.bednbr, cup.*,
(
	select luv.Description
	from cpd_azdelta_prod..LUValue luv with (nolock)
	inner join cpd_azdelta_prod..lutable lut with (nolock) on lut.LUTableID = luv.LUTableID
	where lut.TableName = 'BEDTYPE' and luv.LUValueID = b.bedtype
) as BedTypeDescription,
dr.administrativeid as ResponsibleAdministrativeId, dr.LastName as ResponsibleDoctorLastName, dr.FirstName as ResponsibleDoctorFirstName,
cd.departmentnbr, cd.description as ConsultDepartmentDescription,
(
	select luv.ExtraInfo
	from cpd_azdelta_prod..LUValue luv with (nolock)
	inner join cpd_azdelta_prod..lutable lut with (nolock) on lut.LUTableID = luv.LUTableID
	where lut.TableName = 'LuBedIndex' and luv.LUValueID = cup.lubedindexid
) as DepartmentCode,
(
	select luv.Description
	from cpd_azdelta_prod..LUValue luv with (nolock)
	inner join cpd_azdelta_prod..lutable lut with (nolock) on lut.LUTableID = luv.LUTableID
	where lut.TableName = 'LuBedIndex' and luv.LUValueID = cup.lubedindexid
) as BedIndex,
(
	select luv.Description
	from cpd_azdelta_prod..LUValue luv with (nolock)
	inner join cpd_azdelta_prod..lutable lut with (nolock) on lut.LUTableID = luv.LUTableID
	where lut.TableName = 'RoomFare' and luv.LUValueID = cup.roomfareid
) as RoomFareDescription,
(
	select luv.Description
	from cpd_azdelta_prod..LUValue luv with (nolock)
	inner join cpd_azdelta_prod..lutable lut with (nolock) on lut.LUTableID = luv.LUTableID
	where lut.TableName = 'ReasonRoomFare' and luv.LUValueID = cup.reasonroomfareid
) as ReasonRoomFareDescription
from cpd_azdelta_prod..cpdcareunitperiod cup with (nolock)
inner join cpd_azdelta_prod..cpddossier d with (nolock) on cup.cpddossierid = d.cpddossierid
inner join cpd_azdelta_prod..cpdpatient p with (nolock) on p.cpdpatientid = d.cpdpatientid
left outer join cpd_azdelta_prod..cpdcampus c with (nolock) on c.cpdcampusid = cup.campusid
left outer join cpd_azdelta_prod..cpdcareunithosp cu with (nolock) on cu.cpdcareunithospid = cup.careunitid
left outer join cpd_azdelta_prod..cpdroom r with (nolock) on r.cpdroomid = cup.roomid
left outer join cpd_azdelta_prod..cpdbed b with (nolock) on b.cpdbedid = cup.bedid
left outer join cpd_azdelta_prod..cpddoctor dr with (nolock) on dr.cpddoctorid = cup.doctorid 
left outer join cpd_azdelta_prod..consultdepartment cd with (nolock) on cd.consultdepartmentid = cup.consultdepartmentid 
where d.dossiernr in (@0)
order by cup.startdate");
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, (from number in numbers as IEnumerable<string> select number.PadLeft(8, '0')));
                            }
                        });
                        doctorsByNumbersQuery.Add(key, async (numbers) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = string.Format(@"select dr.*
from cpd_azdelta_prod..cpddoctor dr with (nolock)
where dr.administrativeid in (@0)");
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, (from number in numbers as IEnumerable<string> select number.PadLeft(6, '0')));
                            }
                        });
                        break;
                    case "Aexis":
                        patientByIdQueries.Add(key, async (id) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = @"select *
from MLINE.m_patient
where code = @0";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, id.PadLeft(10, '0'));
                            }
                        });
                        visitsByPatientIdQueries.Add(key, async (id) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = @"select v.*
from MLINE.m_patvisit v
inner join MLINE.m_patient p on p.id = v.patid
where p.code = @0
order by v.admitdate";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, id.PadLeft(10, '0'));
                            }
                        });
                        patientsByNameQueries.Add(key, async (lastName, firstName) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = @"select top 100 *
from MLINE.m_patient
where lower(name) like @0 and lower(fname) like @1
order by name, fname";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, lastName.ToLower() + "%", firstName.ToLower() + "%");
                            }
                        });
                        visitsByNumbersQueries.Add(key, async (numbers) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = @"select *
from MLINE.m_patvisit
where visitnumber in (@0)";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, (from number in numbers as IEnumerable<string> select number.PadLeft(8, '0')));
                            }
                        });
                        patientsByVisitNumbersQueries.Add(key, async (numbers) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = @"select p.*
from MLINE.m_patient p
inner join MLINE.m_patvisit v on v.patid = p.id
where v.code in (@0)";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, (from number in numbers as IEnumerable<string> select number.PadLeft(8, '0')));
                            }
                        });
                        visitDetailsByVisitNumbersQuery.Add(key, async (numbers) =>
                        {
                            return await visitsByNumbersQueries[key](numbers);
                        });
                        break;
                    case "Acertis":
                        patientByIdQueries.Add(key, async (id) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = @"select *
from stammdaten
where kis_patid = @0";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, id.PadLeft(10, '0'));
                            }
                        });
                        visitsByPatientIdQueries.Add(key, async (id) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = @"select v.*
from behandlungsfaelle v
inner join stammdaten p on p.patid = v.patid
where p.kis_patid = @0
order by v.aufnahme_datum";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, id.PadLeft(10, '0'));
                            }
                        });
                        patientsByNameQueries.Add(key, async (lastName, firstName) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = @"select top 100 *
from stammdaten
where lower(name) like @0 and lower(vorname) like @1
order by name, vorname";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, lastName.ToLower() + "%", firstName.ToLower() + "%");
                            }
                        });
                        visitsByNumbersQueries.Add(key, async (numbers) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = @"select v.*
from behandlungsfaelle v
where v.aufnahme_nr in (@0)
order by v.aufnahme_datum";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, (from number in numbers as IEnumerable<string> select number.PadLeft(8, '0')));
                            }
                        });
                        patientsByVisitNumbersQueries.Add(key, async (numbers) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = @"select p.*
from stammdaten p
inner join behandlungsfaelle v on v.patid =  p.patid
where v.aufnahme_nr in (@0)";
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, (from number in numbers as IEnumerable<string> select number.PadLeft(8, '0')));
                            }
                        });
                        break;
                    case "Infohos COZO":
                        var patCozoSql = @"select l.*
from CPDGzo054_PROD..LOG l with (nolock)
inner join CPDGzo054_PROD..LOGArgument la with (nolock) on la.logid = l.logid and la.logkey = 'eventid'
where convert(uniqueidentifier, la.logvalue) in
(
	select ea.EventID
	from CPDGzo054_PROD..EventArgument ea with (nolock) 
	where ea.ArgumentKey = 'cpdpatientid' and ea.isdeleted = 0 and
	convert(uniqueidentifier, ea.ArgumentValue) in (@0)
)
order by l.InsertDt desc";
                        var patCpdSql = @"select p.CPDPatientID
	from cpd_azdelta_prod..cpdpatient p with (nolock)
	where p.PatientNr in (@0)";
                        var visitCozoSql = @"select l.*
from CPDGzo054_PROD..LOG l with (nolock)
inner join CPDGzo054_PROD..LOGArgument la with (nolock) on la.logid = l.logid and la.logkey = 'eventid'
where convert(uniqueidentifier, la.logvalue) in
(
	select ea.EventID
	from CPDGzo054_PROD..EventArgument ea with (nolock) 
	where ea.ArgumentKey = 'cpdcontactid' and ea.isdeleted = 0 and
	convert(uniqueidentifier, ea.ArgumentValue) in (@0)
)
order by l.InsertDt desc";
                        var visitCpdSql = @"select em.OrdContactID
from cpd_azdelta_prod..emergemergency em with (nolock)
inner join cpd_azdelta_prod..CPDDossier d with (nolock) on d.cpddossierid = em.cpddossierid 
where d.DossierNr in (@0)
union
select oc.OrdContactID
from cpd_azdelta_prod..OrdContact oc with (nolock)
inner join cpd_azdelta_prod..CPDDossier d with (nolock) on d.cpddossierid = oc.DossierId 
where d.DossierNr in (@0)
union
select doc.OrdContactID
from cpd_azdelta_prod..cpddossierjordcontact doc with (nolock)
inner join cpd_azdelta_prod..CPDDossier d with (nolock) on d.cpddossierid = doc.cpddossierid 
where d.DossierNr in (@0)";
                        patientByIdQueries.Add(key, async (id) =>
                        {
                            using (var dbCpd = new Database("Infohos CPD"))
                            {
                                Console.WriteLine("{0}: {1}", "Infohos CPD", patCpdSql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                var guids = await dbCpd.FetchAsync<Guid>(patCpdSql, id.PadLeft(10, '0'));

                                using (var db = new Database(key))
                                {
                                    Console.WriteLine("{0}: {1}", key, patCozoSql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                    return await db.FetchAsync<dynamic>(patCozoSql, guids);
                                }
                            }
                        });
                        visitsByPatientIdQueries.Add(key, async (id) =>
                        {
                            using (var dbCpd = new Database("Infohos CPD"))
                            {
                                Console.WriteLine("{0}: {1}", "Infohos CPD", patCpdSql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                var guids = await dbCpd.FetchAsync<Guid>(patCpdSql, id.PadLeft(10, '0'));

                                using (var db = new Database(key))
                                {
                                    Console.WriteLine("{0}: {1}", key, patCozoSql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                    return await db.FetchAsync<dynamic>(patCozoSql, guids);
                                }
                            }
                        });
                        //patientsByNameQueries.Add(key, async (lastName, firstName) =>
                        //{
                        //    using (var db = new Database(key))
                        //    {
                        //        var sql = string.Format(@"");
                        //        Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                        //        return await db.FetchAsync<dynamic>(sql, lastName + "%", firstName + "%");
                        //    }
                        //});
                        visitsByNumbersQueries.Add(key, async (numbers) =>
                        {
                            using (var dbCpd = new Database("Infohos CPD"))
                            {
                                Console.WriteLine("{0}: {1}", "Infohos CPD", visitCpdSql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                var guids = await dbCpd.FetchAsync<Guid>(visitCpdSql, (from number in numbers as IEnumerable<string> select number.PadLeft(8, '0')));

                                using (var db = new Database(key))
                                {
                                    Console.WriteLine("{0}: {1}", key, visitCozoSql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                    return await db.FetchAsync<dynamic>(visitCozoSql, guids);
                                }
                            }
                        });
                        patientsByVisitNumbersQueries.Add(key, async (numbers) =>
                        {
                            using (var dbCpd = new Database("Infohos CPD"))
                            {
                                Console.WriteLine("{0}: {1}", "Infohos CPD", visitCpdSql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                var guids = await dbCpd.FetchAsync<Guid>(visitCpdSql, (from number in numbers as IEnumerable<string> select number.PadLeft(8, '0')));

                                using (var db = new Database(key))
                                {
                                    Console.WriteLine("{0}: {1}", key, visitCozoSql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                    return await db.FetchAsync<dynamic>(visitCozoSql, guids);
                                }
                            }
                        });
                        visitDetailsByVisitNumbersQuery.Add(key, async (numbers) =>
                        {
                            using (var dbCpd = new Database("Infohos CPD"))
                            {
                                Console.WriteLine("{0}: {1}", "Infohos CPD", visitCpdSql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                var guids = await dbCpd.FetchAsync<Guid>(visitCpdSql, (from number in numbers as IEnumerable<string> select number.PadLeft(8, '0')));

                                using (var db = new Database(key))
                                {
                                    Console.WriteLine("{0}: {1}", key, visitCozoSql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                    return await db.FetchAsync<dynamic>(visitCozoSql, guids);
                                }
                            }
                        });
                        //doctorsByNumbersQuery.Add(key, async (numbers) =>
                        //{
                        //    using (var db = new Database(key))
                        //    {
                        //        var sql = string.Format(@"");
                        //        Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                        //        return await db.FetchAsync<dynamic>(sql, (from number in numbers as IEnumerable<string> select number.PadLeft(6, '0')));
                        //    }
                        //});
                        break;
                    case "CMeal":
                        break;
                    case "Allgeier":
                        break;
                    default:
                        continue;
                }

                //propertyWindows.Add(key, new PropertyWindow());
                //propertyWindows[key].Text = key + " Detail";

                //menuItem = new MenuItem();
                //menuItem.Text = key + " Detail";
                //menuItem.Click += new EventHandler(menuItemViewWindow_Click);
                //this.menuItemView.MenuItems.Add(menuItem);
                //menuItem.Index = menuItemViewIndex++;

                listWindows.Add(key, new ListWindow());
                listWindows[key].Text = key + " DB Result";
                listWindows[key].List += new HHR.HL7.Search.ListWindow.ListEventHandler(m_listWindow_List);
                listWindows[key].SearchPatientId += new HHR.HL7.Search.ListWindow.SearchPatientIdEventHandler(SearchPatientId);
                listWindows[key].SearchVisitNumber += new HHR.HL7.Search.ListWindow.SearchVisitNumberEventHandler(SearchVisitNumber);
                listWindows[key].SearchDoctorNumber += new HHR.HL7.Search.ListWindow.SearchDoctorNumberEventHandler(SearchDoctorNumber);

                menuItem = new MenuItem();
                menuItem.Text = key + " Result";
                menuItem.Click += new EventHandler(menuItemViewWindow_Click);
                this.menuItemView.MenuItems.Add(menuItem);
                menuItem.Index = menuItemViewIndex++;
            }

            menuItemViewIndex++;
            foreach (var key in hl7Versions)
            {
                switch (key)
                {
                    case "HL7v21":
                    case "HL7v23":
                        patientByIdQueries.Add(key, async (id) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = string.Format(@"select top 10000 h.MessageTimeStamp, h.MessageType, h.EventType, a.*, h.Message, h.FileName, h.Creator
from {0}..adt a with (nolock)
inner join {0}..hl7 h with (nolock) on h.MessageControlId = a.MessageControlId
where a.PatientId = @0
order by a.MessageControlId", key);
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, id);
                            }
                        });
                        visitsByPatientIdQueries.Add(key, async (id) =>
                        {
                            return await patientByIdQueries[key](id);
                        });
                        patientsByNameQueries.Add(key, async (lastName, firstName) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = string.Format(@"select top 10000 h.MessageTimeStamp, h.MessageType, h.EventType, a.*, h.Message, h.FileName, h.Creator
from {0}..adt a with (nolock)
inner join {0}..hl7 h with (nolock) on h.MessageControlId = a.MessageControlId
where a.FamilyName like @0 and a.FirstName like @1
order by a.MessageControlId", key);
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, lastName + "%", firstName + "%");
                            }
                        });
                        visitsByNumbersQueries.Add(key, async (numbers) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = string.Format(@"select top 10000 h.MessageTimeStamp, h.MessageType, h.EventType, a.*, h.Message, h.FileName, h.Creator
from {0}..adt a with (nolock)
inner join {0}..hl7 h with (nolock) on h.MessageControlId = a.MessageControlId
where a.VisitNumber in (@0)
order by a.MessageControlId", key);
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, (from number in numbers as IEnumerable<string> select number.ToDecimal()));
                            }
                        });
                        patientsByVisitNumbersQueries.Add(key, async (numbers) =>
                        {
                            return await visitsByNumbersQueries[key](numbers);
                        });
                        visitDetailsByVisitNumbersQuery.Add(key, async (numbers) =>
                        {
                            return await visitsByNumbersQueries[key](numbers);
                        });
                        doctorsByNumbersQuery.Add(key, async (numbers) =>
                        {
                            using (var db = new Database(key))
                            {
                                var sql = string.Format(@"select top 10000 h.MessageTimeStamp, h.MessageType, h.EventType, m.*, h.Message, h.FileName, h.Creator
from {0}..mfn m with (nolock)
inner join {0}..hl7 h with (nolock) on h.MessageControlId = m.MessageControlId
where m.DoctorNumber in (@0)
order by m.MessageControlId", key);
                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                                return await db.FetchAsync<dynamic>(sql, (from number in numbers as IEnumerable<string> select number.ToDecimal()));
                            }
                        });
                        //searchAdtQuery.Add(key, async (eventArgs) =>
                        //{

                        //});
                        break;
                    //                    case "Biztalk":
                    //                    case "Biztalk Test":
                    //                        patientByIdQueries.Add(key, async (id) =>
                    //                        {
                    //                            using (var db = new Database(key))
                    //                            {
                    //                                var sql = string.Format(@"select top 1000 mi.CreationDate, mi.SendTransportType, mi.ReceivePortName, mi.ReceiveMessageName,
                    //	(select top 1 value from {0}.BizTalk.Metadata  where [key] = 'SubType' and MessageId = mi.SendMessageid) as SubType,
                    //	(select top 1 value from {0}.BizTalk.Metadata  where [key] = 'PatientNumber' and MessageId = mi.SendMessageid) as PatientNumber,
                    //	(select top 1 value from {0}.BizTalk.Metadata  where [key] = 'VisitNumber' and MessageId = mi.SendMessageid) as VisitNumber,
                    //	(select top 1 value from {0}.BizTalk.Metadata  where [key] = 'StaffNumber' and MessageId = mi.SendMessageid) as StaffNumber, 
                    //	sendMessage.Content as Message
                    //FROM {0}.BizTalk.MessageInstance mi with (nolock)
                    //inner join {0}.BizTalk.Message sendMessage on sendMessage.MessageId = mi.SendMessageid
                    //left outer join {0}.BizTalk.Metadata sendMeta on sendMeta.MessageId = mi.SendMessageid
                    //where mi.sendPortname = @0", key == "Biztalk" ? "Messagebox054" : "Messagebox_Beta");
                    //                                var args = new List<object>() { biztalk };
                    //                                if (!showOnlyLast100BiztalkHL7s)
                    //                                {
                    //                                    sql = string.Concat(sql, Environment.NewLine, @"and sendMeta.[key] = @1
                    //and sendMeta.value = @2");
                    //                                    args.Add("PatientNumber");
                    //                                    args.Add(id);
                    //                                }
                    //                                sql = string.Concat(sql, Environment.NewLine, @"order by mi.CreationDate desc");
                    //                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                    //                                return await db.FetchAsync<dynamic>(sql, args.ToArray());
                    //                            } 
                    //                        });
                    //                        visitsByPatientIdQueries.Add(key, async (id) =>
                    //                        {
                    //                            return await patientByIdQueries[key](id);
                    //                        });
                    //                        visitsByNumbersQueries.Add(key, async (numbers) =>
                    //                        {
                    //                            using (var db = new Database(key))
                    //                            {
                    //                                var sql = string.Format(@"select top 1000 mi.CreationDate, mi.SendTransportType, mi.ReceivePortName, mi.ReceiveMessageName,
                    //	(select top 1 value from {0}.BizTalk.Metadata  where [key] = 'SubType' and MessageId = mi.SendMessageid) as SubType,
                    //	(select top 1 value from {0}.BizTalk.Metadata  where [key] = 'PatientNumber' and MessageId = mi.SendMessageid) as PatientNumber,
                    //	(select top 1 value from {0}.BizTalk.Metadata  where [key] = 'VisitNumber' and MessageId = mi.SendMessageid) as VisitNumber,
                    //	(select top 1 value from {0}.BizTalk.Metadata  where [key] = 'StaffNumber' and MessageId = mi.SendMessageid) as StaffNumber, 
                    //	sendMessage.Content as Message
                    //FROM {0}.BizTalk.MessageInstance mi with (nolock)
                    //inner join {0}.BizTalk.Message sendMessage on sendMessage.MessageId = mi.SendMessageid
                    //left outer join {0}.BizTalk.Metadata sendMeta on sendMeta.MessageId = mi.SendMessageid
                    //where mi.sendPortname = @0", key == "Biztalk" ? "Messagebox054" : "Messagebox_Beta");
                    //                                var args = new List<object>() { biztalk };
                    //                                if (!showOnlyLast100BiztalkHL7s)
                    //                                {
                    //                                    sql = string.Concat(sql, Environment.NewLine, @"and sendMeta.[key] = @1
                    //and sendMeta.value in (@2)");
                    //                                    args.Add("VisitNumber");
                    //                                    args.Add(numbers);
                    //                                }
                    //                                sql = string.Concat(sql, Environment.NewLine, @"order by mi.CreationDate desc");
                    //                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                    //                                return await db.FetchAsync<dynamic>(sql, args.ToArray());
                    //                            }
                    //                        });
                    //                        patientsByVisitNumbersQueries.Add(key, async (numbers) =>
                    //                        {
                    //                            return await visitsByNumbersQueries[key](numbers);
                    //                        });
                    //                        visitDetailsByVisitNumbersQuery.Add(key, async (numbers) =>
                    //                        {
                    //                            return await visitsByNumbersQueries[key](numbers);
                    //                        });
                    //                        doctorsByNumbersQuery.Add(key, async (numbers) =>
                    //                        {
                    //                            using (var db = new Database(key))
                    //                            {
                    //                                var sql = string.Format(@"select top 1000 mi.CreationDate, mi.SendTransportType, mi.ReceivePortName, mi.ReceiveMessageName,
                    //	(select top 1 value from {0}.BizTalk.Metadata  where [key] = 'SubType' and MessageId = mi.SendMessageid) as SubType,
                    //	(select top 1 value from {0}.BizTalk.Metadata  where [key] = 'PatientNumber' and MessageId = mi.SendMessageid) as PatientNumber,
                    //	(select top 1 value from {0}.BizTalk.Metadata  where [key] = 'VisitNumber' and MessageId = mi.SendMessageid) as VisitNumber,
                    //	(select top 1 value from {0}.BizTalk.Metadata  where [key] = 'StaffNumber' and MessageId = mi.SendMessageid) as StaffNumber, 
                    //	sendMessage.Content as Message
                    //FROM {0}.BizTalk.MessageInstance mi with (nolock)
                    //inner join {0}.BizTalk.Message sendMessage on sendMessage.MessageId = mi.SendMessageid
                    //left outer join {0}.BizTalk.Metadata sendMeta on sendMeta.MessageId = mi.SendMessageid
                    //where mi.sendPortname = @0", key == "Biztalk" ? "Messagebox054" : "Messagebox_Beta");
                    //                                var args = new List<object>() { biztalk };
                    //                                if (!showOnlyLast100BiztalkHL7s)
                    //                                {
                    //                                    sql = string.Concat(sql, Environment.NewLine, @"and sendMeta.[key] = @1
                    //and sendMeta.value in (@2)");
                    //                                    args.Add("StaffNumber");
                    //                                    args.Add(numbers);
                    //                                }
                    //                                sql = string.Concat(sql, Environment.NewLine, @"order by mi.CreationDate desc");
                    //                                Console.WriteLine("{0}: {1}", key, sql.Replace(Environment.NewLine, Environment.NewLine + "\t"));
                    //                                return await db.FetchAsync<dynamic>(sql, args.ToArray());
                    //                            }
                    //                        });
                    //                        break;
                }

                treeWindows.Add(key, new HL7TreeWindow());
                treeWindows[key].Text = key + " Tree";

                menuItem = new MenuItem();
                menuItem.Text = key + " Tree";
                menuItem.Click += new EventHandler(menuItemViewWindow_Click);
                this.menuItemView.MenuItems.Add(menuItem);
                menuItem.Index = menuItemViewIndex++;

                syntaxHighlightingWindows.Add(key, new SyntaxHighlightingWindow(key));
                syntaxHighlightingWindows[key].Text = key + " Message";
                syntaxHighlightingWindows[key].SearchPatientId += new HHR.HL7.Search.SyntaxHighlightingWindow.SearchPatientIdEventHandler(SearchPatientId);
                syntaxHighlightingWindows[key].SearchVisitNumber += new HHR.HL7.Search.SyntaxHighlightingWindow.SearchVisitNumberEventHandler(SearchVisitNumber);

                menuItem = new MenuItem();
                menuItem.Text = key + " Message";
                menuItem.Click += new EventHandler(menuItemViewWindow_Click);
                this.menuItemView.MenuItems.Add(menuItem);
                menuItem.Index = menuItemViewIndex++;

                listWindows.Add(key, new ListWindow());
                listWindows[key].Text = key + " HL7 Result";
                listWindows[key].List += new HHR.HL7.Search.ListWindow.ListEventHandler(m_listWindow_List);
                listWindows[key].SearchPatientId += new HHR.HL7.Search.ListWindow.SearchPatientIdEventHandler(SearchPatientId);
                listWindows[key].SearchVisitNumber += new HHR.HL7.Search.ListWindow.SearchVisitNumberEventHandler(SearchVisitNumber);

                menuItem = new MenuItem();
                menuItem.Text = key + " Result";
                menuItem.Click += new EventHandler(menuItemViewWindow_Click);
                this.menuItemView.MenuItems.Add(menuItem);
                menuItem.Index = menuItemViewIndex++;
            }

            m_searchWindow = new SearchWindow(patientByIdQueries.Keys.ToList());
            m_searchHL7ADTWindow = new SearchHL7ADTWindow(hl7Versions);
            m_searchHL7MFNWindow = new SearchHL7MFNWindow(hl7Versions);

            m_outputWindow = new OutputWindow();
            m_exportHL7Form = new ExportHL7Form(hl7Versions);

            dockPanel.AllowEndUserDocking = false;
            m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);

            //dockable panels settings
            m_searchWindow.SearchByPatientName += new HHR.HL7.Search.SearchWindow.SearchByPatientNameEventHandler(m_searchWindow_SearchByPatientName);
            m_searchWindow.SearchByPatientId += new HHR.HL7.Search.SearchWindow.SearchByPatientIdEventHandler(m_searchWindow_SearchByPatientId);
            m_searchWindow.SearchByVisitNumbers += new HHR.HL7.Search.SearchWindow.SearchByVisitNumbersEventHandler(m_searchWindow_SearchByVisitNumbers);
            m_searchWindow.SearchByDoctorNumbers += new HHR.HL7.Search.SearchWindow.SearchByDoctorNumbersEventHandler(m_searchWindow_SearchByDoctorNumbers);
            m_searchWindow.SearchByWardIds += m_searchWindow_SearchByWardIds;
            m_searchWindow.DalChecked += m_searchWindow_DalChecked;
            m_searchHL7ADTWindow.Search += new HHR.HL7.Search.SearchHL7ADTWindow.SearchEventHandler(m_searchHL7ADTWindow_Search);
            m_searchHL7MFNWindow.Search += new HHR.HL7.Search.SearchHL7MFNWindow.SearchEventHandler(m_searchHL7MFNWindow_Search);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.menuItemView = new System.Windows.Forms.MenuItem();
            this.menuItemSearchByPatientIdOrVisitNumberWindow = new System.Windows.Forms.MenuItem();
            this.menuItemSearchHL7ADT = new System.Windows.Forms.MenuItem();
            this.menuItemSearchHL7MFN = new System.Windows.Forms.MenuItem();
            this.menuItem10 = new System.Windows.Forms.MenuItem();
            this.menuItem11 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItemOutput = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItemExportHL7 = new System.Windows.Forms.MenuItem();
            this.menuItem9 = new System.Windows.Forms.MenuItem();
            this.menuItemToolbar = new System.Windows.Forms.MenuItem();
            this.menuItemStatusbar = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.menuItemDefaultLayout = new System.Windows.Forms.MenuItem();
            this.menuItemTools = new System.Windows.Forms.MenuItem();
            this.menuItemLockLayout = new System.Windows.Forms.MenuItem();
            this.menuItemWindow = new System.Windows.Forms.MenuItem();
            this.menuItemHelp = new System.Windows.Forms.MenuItem();
            this.menuItemAbout = new System.Windows.Forms.MenuItem();
            this.statusBar = new System.Windows.Forms.StatusBar();
            this.statusBarPanel = new System.Windows.Forms.StatusBarPanel();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.toolBar = new System.Windows.Forms.ToolBar();
            this.toolBarButtonSearch = new System.Windows.Forms.ToolBarButton();
            this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
			this.menuItemView,
			this.menuItemTools,
			this.menuItemWindow,
			this.menuItemHelp});
            // 
            // menuItemView
            // 
            this.menuItemView.Index = 0;
            this.menuItemView.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
			this.menuItemSearchByPatientIdOrVisitNumberWindow,
			this.menuItemSearchHL7ADT,
			this.menuItemSearchHL7MFN,
			this.menuItem10,
			this.menuItem11,
			this.menuItem2,
			this.menuItemOutput,
			this.menuItem1,
			this.menuItemExportHL7,
			this.menuItem9,
			this.menuItemToolbar,
			this.menuItemStatusbar,
			this.menuItem8,
			this.menuItemDefaultLayout});
            this.menuItemView.Text = "&View";
            // 
            // menuItemSearchByPatientIdOrVisitNumberWindow
            // 
            this.menuItemSearchByPatientIdOrVisitNumberWindow.Index = 0;
            this.menuItemSearchByPatientIdOrVisitNumberWindow.Text = "&Search by PatientId/VisitNumber";
            this.menuItemSearchByPatientIdOrVisitNumberWindow.Click += new System.EventHandler(this.menuItemSearchByPatientIdOrVisitNumberWindow_Click);
            // 
            // menuItemSearchHL7ADT
            // 
            this.menuItemSearchHL7ADT.Index = 1;
            this.menuItemSearchHL7ADT.Text = "Search HL7 ADT";
            this.menuItemSearchHL7ADT.Click += new System.EventHandler(this.menuItemSearchHL7ADT_Click);
            // 
            // menuItemSearchHL7MFN
            // 
            this.menuItemSearchHL7MFN.Index = 2;
            this.menuItemSearchHL7MFN.Text = "Search HL7 MFN";
            this.menuItemSearchHL7MFN.Click += new System.EventHandler(this.menuItemSearchHL7MFN_Click);
            // 
            // menuItem10
            // 
            this.menuItem10.Index = 3;
            this.menuItem10.Text = "-";
            // 
            // menuItem11
            // 
            this.menuItem11.Index = 4;
            this.menuItem11.Text = "-";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 5;
            this.menuItem2.Text = "-";
            // 
            // menuItemOutput
            // 
            this.menuItemOutput.Index = 6;
            this.menuItemOutput.Text = "Output";
            this.menuItemOutput.Click += new System.EventHandler(this.menuItemOutput_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 7;
            this.menuItem1.Text = "-";
            // 
            // menuItemExportHL7
            // 
            this.menuItemExportHL7.Index = 8;
            this.menuItemExportHL7.Text = "Export HL7";
            this.menuItemExportHL7.Click += new System.EventHandler(this.menuItemExportHL7_Click);
            // 
            // menuItem9
            // 
            this.menuItem9.Index = 9;
            this.menuItem9.Text = "-";
            // 
            // menuItemToolbar
            // 
            this.menuItemToolbar.Checked = true;
            this.menuItemToolbar.Index = 10;
            this.menuItemToolbar.Text = "Tool &Bar";
            this.menuItemToolbar.Click += new System.EventHandler(this.menuItemToolbar_Click);
            // 
            // menuItemStatusbar
            // 
            this.menuItemStatusbar.Checked = true;
            this.menuItemStatusbar.Index = 11;
            this.menuItemStatusbar.Text = "Status B&ar";
            this.menuItemStatusbar.Click += new System.EventHandler(this.menuItemStatusbar_Click);
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 12;
            this.menuItem8.Text = "-";
            // 
            // menuItemDefaultLayout
            // 
            this.menuItemDefaultLayout.Index = 13;
            this.menuItemDefaultLayout.Text = "Default Layout";
            this.menuItemDefaultLayout.Click += new System.EventHandler(this.menuItemDefaultLayout_Click);
            // 
            // menuItemTools
            // 
            this.menuItemTools.Index = 1;
            this.menuItemTools.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
			this.menuItemLockLayout});
            this.menuItemTools.Text = "&Tools";
            this.menuItemTools.Popup += new System.EventHandler(this.menuItemTools_Popup);
            // 
            // menuItemLockLayout
            // 
            this.menuItemLockLayout.Index = 0;
            this.menuItemLockLayout.Text = "&Lock Layout";
            this.menuItemLockLayout.Click += new System.EventHandler(this.menuItemLockLayout_Click);
            // 
            // menuItemWindow
            // 
            this.menuItemWindow.Index = 2;
            this.menuItemWindow.Text = "&Window";
            // 
            // menuItemHelp
            // 
            this.menuItemHelp.Index = 3;
            this.menuItemHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
			this.menuItemAbout});
            this.menuItemHelp.Text = "&Help";
            // 
            // menuItemAbout
            // 
            this.menuItemAbout.Index = 0;
            this.menuItemAbout.Text = "&About HHR HL7 Search...";
            this.menuItemAbout.Click += new System.EventHandler(this.menuItemAbout_Click);
            // 
            // statusBar
            // 
            this.statusBar.Location = new System.Drawing.Point(0, 328);
            this.statusBar.Name = "statusBar";
            this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
			this.statusBarPanel});
            this.statusBar.ShowPanels = true;
            this.statusBar.Size = new System.Drawing.Size(579, 22);
            this.statusBar.TabIndex = 1;
            // 
            // statusBarPanel
            // 
            this.statusBarPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.statusBarPanel.Name = "statusBarPanel";
            this.statusBarPanel.Width = 563;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "");
            // 
            // toolBar
            // 
            this.toolBar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.toolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
			this.toolBarButtonSearch});
            this.toolBar.DropDownArrows = true;
            this.toolBar.ImageList = this.imageList;
            this.toolBar.Location = new System.Drawing.Point(0, 0);
            this.toolBar.Name = "toolBar";
            this.toolBar.ShowToolTips = true;
            this.toolBar.Size = new System.Drawing.Size(579, 28);
            this.toolBar.TabIndex = 2;
            this.toolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar_ButtonClick);
            // 
            // toolBarButtonSearch
            // 
            this.toolBarButtonSearch.ImageIndex = 0;
            this.toolBarButtonSearch.Name = "toolBarButtonSearch";
            // 
            // dockPanel
            // 
            this.dockPanel.ActiveAutoHideContent = null;
            this.dockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World, ((byte)(0)));
            this.dockPanel.Location = new System.Drawing.Point(0, 28);
            this.dockPanel.Name = "dockPanel";
            this.dockPanel.Size = new System.Drawing.Size(579, 300);
            this.dockPanel.TabIndex = 4;
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(579, 350);
            this.Controls.Add(this.dockPanel);
            this.Controls.Add(this.toolBar);
            this.Controls.Add(this.statusBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Menu = this.mainMenu;
            this.Name = "MainForm";
            this.Text = "HHR HL7 Search";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        #region Methods

        async Task<IList<dynamic>> HttpGet(string connectionString, string requestUri)
        {
            using (var client = CreateHttpClient(new Uri(connectionString)))
            {
                var response = await client.GetAsync(requestUri).ConfigureAwait(false);
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return await Task.Run(() =>
                {
                    var result = JsonConvert.DeserializeObject<ExpandoObject>(content, new ExpandoObjectConverter());

                    if (result == null || !(result is IDictionary<string, object>) || !(result as IDictionary<string, object>).ContainsKey("rows"))
                        return null;

                    if (((dynamic)result).rows.Count == 0)
                        return null;

                    return (from row in ((dynamic)result).rows as IEnumerable<dynamic>
                            where ((IDictionary<string, Object>)row).ContainsKey("doc")
                            select row.doc).ToList();
                }).ConfigureAwait(false);
            }
        }

        static HttpClient CreateHttpClient(Uri uri)
        {
            var client = new HttpClient(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            })
            {
                BaseAddress = new Uri(uri.GetAbsoluteUriExceptUserInfo().TrimEnd(new[] { '/' })),
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("gzip"));
            client.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("deflate"));

            var basicAuthString = uri.GetBasicAuthString();
            if (basicAuthString != null)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuthString);

            return client;
        }

        IDockContent GetContentFromPersistString(string persistString)
        {
            string[] parsedStrings = persistString.Split(new char[] { ',' });
            string key;

            if (parsedStrings.Length < 1)
                return null;

            if (parsedStrings[0] == typeof(SearchWindow).ToString())
                return m_searchWindow;
            else if (parsedStrings[0] == typeof(OutputWindow).ToString())
                return m_outputWindow;
            else if (parsedStrings[0] == typeof(ExportHL7Form).ToString())
                return m_exportHL7Form;
            else if (parsedStrings[0] == typeof(SearchHL7ADTWindow).ToString())
                return m_searchHL7ADTWindow;
            else if (parsedStrings[0] == typeof(SearchHL7MFNWindow).ToString())
                return m_searchHL7MFNWindow;
            else if (parsedStrings[0] == typeof(ListWindow).ToString())
            {
                key = parsedStrings[1].Substring(0, parsedStrings[1].IndexOf(" Result"));

                if (listWindows.ContainsKey(key))
                    return listWindows[key];
                else
                    return null;
            }
            else if (parsedStrings[0] == typeof(PropertyWindow).ToString())
            {
                key = parsedStrings[1].Substring(0, parsedStrings[1].IndexOf(" Detail"));

                if (propertyWindows.ContainsKey(key))
                    return propertyWindows[key];
                else
                    return null;
            }
            else if (parsedStrings[0] == typeof(SyntaxHighlightingWindow).ToString())
            {
                key = parsedStrings[1].Substring(0, parsedStrings[1].IndexOf(" Message"));

                if (syntaxHighlightingWindows.ContainsKey(key))
                    return syntaxHighlightingWindows[key];
                else
                    return null;
            }
            else if (parsedStrings[0] == typeof(HL7TreeWindow).ToString())
            {
                key = parsedStrings[1].Substring(0, parsedStrings[1].IndexOf(" Tree"));

                if (treeWindows.ContainsKey(key))
                    return treeWindows[key];
                else
                    return null;
            }

            return null;
        }

        void ProgrammaticalyLoadDockpanel()
        {
            CloseAllContents();

            dockPanel.SuspendLayout();

            dockPanel.DockLeftPortion = 0.20;

            m_searchWindow.Show(dockPanel, DockState.DockLeftAutoHide);
            //m_searchHL7ADTWindow.Show(dockPanel, DockState.DockLeftAutoHide);
            //m_searchHL7MFNWindow.Show(dockPanel, DockState.DockLeftAutoHide);
            m_exportHL7Form.Show(dockPanel, DockState.DockLeftAutoHide);

            m_outputWindow.Show(dockPanel, DockState.DockBottomAutoHide);


            dockPanel.DockRightPortion = 0.10;
            dockPanel.DockLeftPortion = 0.90;

            ListWindow previousDbListWindow = null;
            int i = 0;

            var checkedDals = (from ListViewItem item in m_searchWindow.listViewDals.CheckedItems
                               where item != null && item.Checked
                               select item.Text).ToList();
            var dbListWindows = (from lw in listWindows
                                 where lw.Value.Text.Contains(" DB Result") && checkedDals.Contains(lw.Value.Text.Substring(0, lw.Value.Text.IndexOf(" DB Result")))
                                 select lw.Value).ToList();
            var hl7ListWindows = (from lw in listWindows
                                  where lw.Value.Text.Contains(" HL7 Result") && checkedDals.Contains(lw.Value.Text.Substring(0, lw.Value.Text.IndexOf(" HL7 Result")))
                                  select lw.Value).ToList();
            var hl7SyntaxHighlightingWindows = (from lw in syntaxHighlightingWindows
                                                let dal = lw.Value.Text.Substring(0, lw.Value.Text.IndexOf(" Message"))
                                                where checkedDals.Contains(dal)
                                                select lw.Value).ToList();
            var hl7TreeWindows = (from lw in treeWindows
                                  let dal = lw.Value.Text.Substring(0, lw.Value.Text.IndexOf(" Tree"))
                                  where checkedDals.Contains(dal)
                                  select lw.Value).ToList();

            foreach (var window in dbListWindows)
            {
                if (previousDbListWindow == null)
                {
                    window.Show(dockPanel, DockState.DockLeft);

                    int j = 0;

                    ListWindow previousHl7ListWindow = null;
                    foreach (var hl7Window in hl7ListWindows)
                    {
                        if (previousHl7ListWindow == null)
                            hl7Window.Show(window.Pane, DockAlignment.Right, 0.5);
                        else
                            hl7Window.Show(previousHl7ListWindow.Pane, DockAlignment.Bottom, 1d - (1d / (hl7ListWindows.Count + hl7SyntaxHighlightingWindows.Count - j + 1)));
                        previousHl7ListWindow = hl7Window;
                        j++;
                    }

                    SyntaxHighlightingWindow previousSyntaxHighlightingWindow = null;
                    foreach (var syntaxHighlightingWindow in hl7SyntaxHighlightingWindows)
                    {
                        if (previousSyntaxHighlightingWindow == null)
                            syntaxHighlightingWindow.Show(previousHl7ListWindow.Pane, DockAlignment.Bottom, 1d - (1d / (hl7ListWindows.Count + hl7SyntaxHighlightingWindows.Count - j + 1)));
                        else
                            syntaxHighlightingWindow.Show(previousSyntaxHighlightingWindow.Pane, DockAlignment.Bottom, 1d - (1d / (hl7ListWindows.Count + hl7SyntaxHighlightingWindows.Count - j + 1)));
                        previousSyntaxHighlightingWindow = syntaxHighlightingWindow;
                        j++;
                    }
                }
                else
                    window.Show(previousDbListWindow.Pane, DockAlignment.Bottom, 1d - (1d / (dbListWindows.Count - i + 1)));
                previousDbListWindow = window;
                i++;
            }

            foreach (var window in hl7TreeWindows)
                window.Show(dockPanel, DockState.DockRightAutoHide);

            m_searchWindow.Show();

            dockPanel.ResumeLayout();
        }

        void CloseAllContents()
        {
            dockPanel.SuspendLayout();

            // we don't want to create another instance of tool window, set DockPanel to null
            foreach (PropertyWindow window in propertyWindows.Values)
                window.DockPanel = null;
            foreach (ListWindow window in listWindows.Values)
                window.DockPanel = null;

            foreach (HL7TreeWindow window in treeWindows.Values)
                window.DockPanel = null;
            foreach (SyntaxHighlightingWindow window in syntaxHighlightingWindows.Values)
                window.DockPanel = null;

            m_searchWindow.DockPanel = null;
            m_searchHL7ADTWindow.DockPanel = null;
            m_searchHL7MFNWindow.DockPanel = null;

            m_outputWindow.DockPanel = null;
            m_exportHL7Form.DockPanel = null;

            // Close all other document windows
            CloseAllDocuments();

            dockPanel.ResumeLayout();
        }

        void CloseAllDocuments()
        {
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                foreach (Form form in MdiChildren)
                    form.Close();
            }
            else
            {
                foreach (IDockContent content in dockPanel.Documents)
                    content.DockHandler.Close();
            }
        }

        Task DoSearch(string key, dynamic data, Func<dynamic, Task<IList<dynamic>>> func)
        {
            return Task.Run(async () =>
            {
                try
                {
                    UpdateResult(key, null);
                    UpdateStatus(key, string.Format("{0} {1} Result, searching...", key, dbs.Contains(key) ? "DB" : "HL7"));
                    var results = await func(data).ConfigureAwait(false);
                    UpdateResult(key, results);
                    if (results == null)
                        UpdateStatus(key, string.Format("{0} {1} Result, 0 results", key, dbs.Contains(key) ? "DB" : "HL7"));
                    else
                        UpdateStatus(key, string.Format("{0} {1} Result, {2} results", key, dbs.Contains(key) ? "DB" : "HL7", results.Count));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("{0}: {1}", key, ex.ToString());
                }
            });
        }

        #endregion

        #region Invoke Methods

        void StatusText(string value)
        {
            this.UIThread(() =>
                {
                    if (this.statusBarPanel != null)
                        statusBarPanel.Text = value;
                });
        }

        void UpdateStatus(string key, string value)
        {
            this.UIThread(() =>
                {
                    if (listWindows.ContainsKey(key))
                        if (listWindows[key] != null)
                            listWindows[key].Text = value;
                });
        }

        void UpdateResult(string key, IList<dynamic> results)
        {
            this.UIThread(() =>
                {
                    if (listWindows.ContainsKey(key))
                        if (listWindows[key] != null)
                            listWindows[key].DisplayResults(results);
                });
        }

        #endregion

        #region Events

        #region Form Events

        void MainForm_Load(object sender, System.EventArgs e)
        {
            Task.Run(async () =>
                {
                    using (var db = new Database("security"))
                    {
                        var sql = @"select count(*)
from security..[user] with (nolock)
where applicationid = 
(
	select id
	from security..application
	where name = 'HL7 Search'
)
and lower(accountname) = @0";
                        if (await db.ExecuteScalarAsync<int>(sql, Environment.UserName.ToLower().Trim()) == 0)
                        {
                            MessageBox.Show("Onvoldoende rechten!", "Onvoldoende rechten", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Application.Exit();
                        }
                    }
                });

            menuItemDefaultLayout_Click(null, null);
        }

        void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CloseAllContents();
            //dockPanel.SaveAsXml(DockPanelConfigFile);
        }

        #endregion

        #region Menu Item Events

        void menuItemViewWindow_Click(object sender, EventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;

            switch (menuItem.Text.Split(' ').Last())
            {
                case "Result":
                    if (listWindows.ContainsKey(menuItem.Text.TrimEnd(" Result")))
                        listWindows[menuItem.Text.TrimEnd(" Result")].Show(dockPanel);
                    break;
                case "Detail":
                    if (propertyWindows.ContainsKey(menuItem.Text.TrimEnd(" Detail")))
                        propertyWindows[menuItem.Text.TrimEnd(" Detail")].Show(dockPanel);
                    break;
                case "Message":
                    if (syntaxHighlightingWindows.ContainsKey(menuItem.Text.TrimEnd(" Message")))
                        syntaxHighlightingWindows[menuItem.Text.TrimEnd(" Message")].Show(dockPanel);
                    break;
                case "Tree":
                    if (treeWindows.ContainsKey(menuItem.Text.TrimEnd(" Tree")))
                        treeWindows[menuItem.Text.TrimEnd(" Tree")].Show(dockPanel);
                    break;
            }
        }

        void menuItemExit_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        void menuItemToolbar_Click(object sender, System.EventArgs e)
        {
            toolBar.Visible = menuItemToolbar.Checked = !menuItemToolbar.Checked;
        }

        void menuItemStatusbar_Click(object sender, System.EventArgs e)
        {
            statusBar.Visible = menuItemStatusbar.Checked = !menuItemStatusbar.Checked;
        }

        void menuItemAbout_Click(object sender, System.EventArgs e)
        {
            AboutDialog aboutDialog = new AboutDialog();
            aboutDialog.ShowDialog(this);
        }

        void menuItemTools_Popup(object sender, System.EventArgs e)
        {
            menuItemLockLayout.Checked = !this.dockPanel.AllowEndUserDocking;
        }

        void menuItemLockLayout_Click(object sender, System.EventArgs e)
        {
            dockPanel.AllowEndUserDocking = !dockPanel.AllowEndUserDocking;
        }

        void menuItemSearchByPatientIdOrVisitNumberWindow_Click(object sender, System.EventArgs e)
        {
            m_searchWindow.Show(dockPanel);
        }

        void menuItemSearchHL7ADT_Click(object sender, System.EventArgs e)
        {
            m_searchHL7ADTWindow.Show(dockPanel);
        }

        void menuItemSearchHL7MFN_Click(object sender, System.EventArgs e)
        {
            m_searchHL7MFNWindow.Show(dockPanel);
        }

        void menuItemOutput_Click(object sender, System.EventArgs e)
        {
            m_outputWindow.Show(dockPanel);
        }

        void menuItemExportHL7_Click(object sender, EventArgs e)
        {
            m_exportHL7Form.Show(dockPanel);
        }

        void menuItemDefaultLayout_Click(object sender, EventArgs e)
        {
            this.SuspendLayout();
            dockPanel.SuspendLayout(true);

            // In order to load layout from XML, we need to close all the DockContents
            CloseAllContents();

            ProgrammaticalyLoadDockpanel();

            dockPanel.ResumeLayout(true, true);
            this.ResumeLayout();
        }

        #endregion

        #region ToolBar Events

        void toolBar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
        {
            if (e.Button == toolBarButtonSearch)
                menuItemSearchByPatientIdOrVisitNumberWindow_Click(null, null);
        }

        #endregion

        #region Dockable Windows Events

        void m_searchWindow_DalChecked(object sender, ItemCheckedEventArgs e)
        {
            //if (listWindows.ContainsKey(e.Item.Text))
            //{
            //    var lw = listWindows[e.Item.Text];
            //    if (e.Item.Checked && lw.IsHidden)
            //        lw.Show(dockPanel);
            //    else if (!e.Item.Checked && !lw.IsHidden)
            //        lw.Hide();
            //}
            //if (syntaxHighlightingWindows.ContainsKey(e.Item.Text))
            //{
            //    var shlw = syntaxHighlightingWindows[e.Item.Text];
            //    if (e.Item.Checked && shlw.IsHidden)
            //        shlw.Show(dockPanel);
            //    else if (!e.Item.Checked && !shlw.IsHidden)
            //        shlw.Hide();
            //} 
            //if (hl7TreeWindows.ContainsKey(e.Item.Text))
            //{
            //    var shlw = hl7TreeWindows[e.Item.Text];
            //    if (e.Item.Checked && shlw.IsHidden)
            //        shlw.Show(dockPanel);
            //    else if (!e.Item.Checked && !shlw.IsHidden)
            //        shlw.Hide();
            //} 

            ProgrammaticalyLoadDockpanel();
        }

        async void m_searchWindow_SearchByPatientName(object sender, SearchByPatientNameEventArgs e)
        {
            biztalk = e.Biztalk;
            showOnlyLast100BiztalkHL7s = e.BiztalkShowOnlyLast100HL7s;
            var tasks = new List<Task>();
            foreach (var key in e.SearchDals)
                if (!patientsByNameQueries.ContainsKey(key))
                {
                    UpdateResult(key, null);
                    UpdateStatus(key, key + " not implemented");
                }
                else
                    tasks.Add(Task.Run(async () =>
                    {
                        try
                        {
                            UpdateResult(key, null);
                            UpdateStatus(key, key + " Result, searching...");
                            var results = await patientsByNameQueries[key](e.PatientFamilyName, e.PatientGivenName).ConfigureAwait(false);
                            UpdateResult(key, results);
                            if (results == null)
                                UpdateStatus(key, string.Format("{0} {1} Result, 0 results", key, dbs.Contains(key) ? "DB" : "HL7"));
                            else
                                UpdateStatus(key, string.Format("{0} {1} Result, {2} results", key, dbs.Contains(key) ? "DB" : "HL7", results.Count));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("{0}: {1}", key, ex.ToString());
                        }
                    }));
            await Task.WhenAll(tasks.ToArray()).ConfigureAwait(false);
        }

        async void m_searchWindow_SearchByPatientId(object sender, SearchByPatientIdEventArgs e)
        {
            biztalk = e.Biztalk;
            showOnlyLast100BiztalkHL7s = e.BiztalkShowOnlyLast100HL7s;
            var tasks = new List<Task>();
            switch (e.ShowState)
            {
                case ShowState.Patients:
                    foreach (var key in e.SearchDals)
                        if (!patientByIdQueries.ContainsKey(key))
                        {
                            UpdateResult(key, null);
                            UpdateStatus(key, key + " not implemented");
                        }
                        else
                            tasks.Add(DoSearch(key, e.PatientId, patientByIdQueries[key]));
                    break;
                case ShowState.Visists:
                    foreach (var key in e.SearchDals)
                        if (!visitsByPatientIdQueries.ContainsKey(key))
                        {
                            UpdateResult(key, null);
                            UpdateStatus(key, key + " not implemented");
                        }
                        else
                            tasks.Add(DoSearch(key, e.PatientId, visitsByPatientIdQueries[key]));
                    break;
            }

            await Task.WhenAll(tasks.ToArray()).ConfigureAwait(false);
        }

        async void m_searchWindow_SearchByVisitNumbers(object sender, SearchByVisitNumbersEventArgs e)
        {
            biztalk = e.Biztalk;
            showOnlyLast100BiztalkHL7s = e.BiztalkShowOnlyLast100HL7s;
            var tasks = new List<Task>();
            switch (e.ShowState)
            {
                case ShowState.Patients:
                    foreach (var key in e.SearchDals)
                        if (!patientsByVisitNumbersQueries.ContainsKey(key))
                        {
                            UpdateResult(key, null);
                            UpdateStatus(key, key + " not implemented");
                        }
                        else
                            tasks.Add(DoSearch(key, e.VisitNumbers, patientsByVisitNumbersQueries[key]));
                    break;
                case ShowState.Visists:
                    foreach (var key in e.SearchDals)
                        if (!visitsByNumbersQueries.ContainsKey(key))
                        {
                            UpdateResult(key, null);
                            UpdateStatus(key, key + " not implemented");
                        }
                        else
                            tasks.Add(DoSearch(key, e.VisitNumbers, visitsByNumbersQueries[key]));
                    break;
                case ShowState.VisitDetails:
                    foreach (var key in e.SearchDals)
                        if (!visitDetailsByVisitNumbersQuery.ContainsKey(key))
                        {
                            UpdateResult(key, null);
                            UpdateStatus(key, key + " not implemented");
                        }
                        else
                            tasks.Add(DoSearch(key, e.VisitNumbers, visitDetailsByVisitNumbersQuery[key]));
                    break;
            }

            await Task.WhenAll(tasks.ToArray()).ConfigureAwait(false);
        }

        async void m_searchWindow_SearchByDoctorNumbers(object sender, SearchByDoctorNumbersEventArgs e)
        {
            biztalk = e.Biztalk;
            showOnlyLast100BiztalkHL7s = e.BiztalkShowOnlyLast100HL7s;
            var tasks = new List<Task>();
            foreach (var key in e.SearchDals)
                if (!doctorsByNumbersQuery.ContainsKey(key))
                {
                    UpdateResult(key, null);
                    UpdateStatus(key, key + " not implemented");
                }
                else
                    tasks.Add(DoSearch(key, e.DoctorNumbers, doctorsByNumbersQuery[key]));

            await Task.WhenAll(tasks.ToArray()).ConfigureAwait(false);
        }

        async void m_searchWindow_SearchByWardIds(object sender, SearchByWardIdsEventArgs e)
        {
            biztalk = e.Biztalk;
            showOnlyLast100BiztalkHL7s = e.BiztalkShowOnlyLast100HL7s;
            var tasks = new List<Task>();
            foreach (var key in e.SearchDals)
                if (!bedOccupationByWardIdsQuery.ContainsKey(key))
                {
                    UpdateResult(key, null);
                    UpdateStatus(key, key + " not implemented");
                }
                else
                    tasks.Add(DoSearch(key, e.WardIds, bedOccupationByWardIdsQuery[key]));

            await Task.WhenAll(tasks.ToArray()).ConfigureAwait(false);
        }

        async void m_searchHL7ADTWindow_Search(object sender, SearchHL7ADTEventArgs e)
        {
            biztalk = e.Biztalk;
            showOnlyLast100BiztalkHL7s = e.BiztalkShowOnlyLast100HL7s;
            var tasks = new List<Task>();
            foreach (var key in e.SearchDals)
                if (!searchAdtQuery.ContainsKey(key))
                {
                    UpdateResult(key, null);
                    UpdateStatus(key, key + " not implemented");
                }
                else
                    tasks.Add(DoSearch(key, e, searchAdtQuery[key]));

            await Task.WhenAll(tasks.ToArray()).ConfigureAwait(false);
        }

        void m_searchHL7MFNWindow_Search(object sender, SearchHL7MFNEventArgs e)
        {
            //FetchData(e);
        }

        void m_listWindow_List(object sender, ListEventArgs e)
        {
            foreach (KeyValuePair<string, ListWindow> kvp in listWindows)
            {
                if (sender == kvp.Value)
                {
                    if (propertyWindows.ContainsKey(kvp.Key))
                    {
                        propertyWindows[kvp.Key].Entity = e.Entity;
                        return;
                    }
                    else
                    {
                        if (syntaxHighlightingWindows.ContainsKey(kvp.Key))
                            syntaxHighlightingWindows[kvp.Key].Entity = e.Entity;
                        if (treeWindows.ContainsKey(kvp.Key))
                            treeWindows[kvp.Key].Entity = e.Entity;
                        return;
                    }
                }
            }
        }

        void SearchPatientId(object sender, PatientIdEventArgs e)
        {
            this.m_searchWindow.PatientId = e.PatientId;
            this.m_searchWindow.Show(this.dockPanel);
            this.m_searchWindow.DoSearch();
        }

        void SearchVisitNumber(object sender, VisitNumberEventArgs e)
        {
            this.m_searchWindow.VisitNumbers = e.VisitNumbers;
            this.m_searchWindow.Show(this.dockPanel);
            this.m_searchWindow.DoSearch();
        }

        void SearchDoctorNumber(object sender, DoctorNumberEventArgs e)
        {
            this.m_searchWindow.DoctorNumbers = e.DoctorNumbers;
            this.m_searchWindow.Show(this.dockPanel);
            this.m_searchWindow.DoSearch();
        }

        #endregion

        #endregion

        #region Delegates

        delegate void StringParameterDelegate(string value);
        delegate void SetTextInvoker(string key, string value);
        delegate void DisplaySetInvoker(string key, System.Collections.IList entitySet);

        #endregion
    }

    #region EventArgs

    public class PatientIdEventArgs : EventArgs
    {
        public PatientIdEventArgs(string patientId)
        {
            PatientId = patientId;
        }

        public string PatientId { get; private set; }
    }

    public class VisitNumberEventArgs : EventArgs
    {
        public VisitNumberEventArgs(string[] visitNumbers)
        {
            VisitNumbers = visitNumbers;
        }

        public string[] VisitNumbers { get; private set; }
    }

    #endregion
}
