using GeoChatter.Helpers;
using GeoChatter.Core.Extensions;
using GeoChatter.Core.Handlers;
using GeoChatter.Core.Helpers;
using GeoChatter.Core.Model;
using GeoChatter.FormUtils;
using GeoChatter.Handlers;
using GeoChatter.Properties;
using log4net;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GeoChatter.Model;
using GeoChatter.Core.Common.Extensions;

namespace GeoChatter.Forms.ScoreCalculator
{
    /// <summary>
    /// Score formulator form
    /// </summary>
    [SupportedOSPlatform("windows7.0")]
    public partial class ScoreFormulator : Form
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(ScoreFormulator));

        private readonly MainForm mainForm;

        /// <summary>
        /// Current expression for calculations
        /// </summary>
        public static ScoreFormulatorExpression CurrentExpression { get; set; } = new();
        /// <summary>
        /// Last expression string that was compiled
        /// </summary>
        public static string LastCompiledExpression { get; set; } = string.IsNullOrWhiteSpace(Settings.Default.ScoreFormula)
            ? (Settings.Default.ScoreFormula = nameof(ScoreFormulatorExpression.DefaultScore))
            : Settings.Default.ScoreFormula;
        /// <summary>
        /// Last executable script compiled
        /// </summary>
        public static Script<object> LastCompiledScript { get; set; } = CSharpScript.Create(LastCompiledExpression, null, CurrentExpression.GetType());
        /// <summary>
        /// Wheter <see cref="CurrentExpression"/> was compiled yet
        /// </summary>
        public static bool Initialized { get; private set; }

        private DataTable datatable { get; } = new();

        private Scintilla TextArea;

        private bool changesMade;
        /// <summary>
        /// Wheter there were any changes made to the <see cref="CurrentExpression"/>
        /// </summary>
        public bool ChangesMade
        {
            get => changesMade;
            set
            {
                Text = value ? "Score Formulator (Unsaved Changes)" : "Score Formulator";
                changesMade = value;
            }
        }

        private static string cachedSvgClassesCss { get; set; }

        private static string Version { get; set; }

        private static string MapHTMLTemplate { get; set; }

        internal static bool IsAvailable { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        public ScoreFormulator(string version)
        {
            if (!IsAvailable)
            {
                return;
            }

            IsAvailable = false;
            Version = version;
            try
            {
                InitializeComponent();
                if (!Initialized)
                {
                    Initialized = true;
                    if (LastCompiledScript != null)
                    {
                        LastCompiledScript.Compile();
                    }
                    InitializeMapHtml();
                }
            }
            finally // TODO: Catch
            {
                IsAvailable = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        public ScoreFormulator(MainForm parent) : this(parent?.Version)
        {
            if (!IsAvailable)
            {
                return;
            }
            mainForm = parent;

            datatable.Columns.Add(nameof(ScoreFormulatorVariableRow.VariableName), typeof(string));
            datatable.Columns.Add(nameof(ScoreFormulatorVariableRow.VariableValue), typeof(ScoreFormulatorVariable));

            datatable.Locale = System.Globalization.CultureInfo.InvariantCulture;

            foreach (PropertyInfo prop in typeof(ScoreFormulatorExpression).GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (!prop.CanWrite)
                {
                    continue;
                }

                datatable.LoadDataRow(new object[]
                {
                    prop.Name,
                    GetPropDefaultValue(prop.Name)
                }, true);
            }

            testVariablesDataGrid.DataSource = new BindingSource() { DataSource = datatable };

            presetFormulasCB.Items.AddRange(Formulas.Keys.Select(k => (object)k).ToArray());

            mainForm.ResetJSCTRLCheck();
        }

        private static ScoreFormulatorVariable GetPropDefaultValue(string name)
        {
            return name == nameof(ScoreFormulatorExpression.MapScale) ? ScoreFormulatorExpression.MapScaleADW : ScoreFormulatorVariable.False;
        }

        private static void InitializeMapHtml()
        {
            try
            {
#if DEBUG
                MapHTMLTemplate = File.ReadAllText(Application.StartupPath + "\\ScoreFormulatorMap.html");
#else
                string url = Path.Combine(ResourceHelper.VersionedScriptServiceURL(Version), "ScoreFormulatorMap.html");
                ResourceHelper.GetWebResource(url,
                    (string content) =>
                    {
                        logger.Info("Fetched score formulator map from " + url);
                        MapHTMLTemplate = content;
                    },
                    (string content, Exception ex) =>
                    {
                        MapHTMLTemplate = "<h1>FAILED TO GET THE MAP PAGE. PLEASE RESTART THE APPLICATION</h1>";
                        logger.Error($"Failed to fetch score formulator map from {url} -> {content} : {ex.Summarize()}");
                    }
                );
#endif
                if (string.IsNullOrWhiteSpace(MapHTMLTemplate))
                {
                    logger.Error("Score formulator map was empty");
                    return;
                }
                Dictionary<string, string> reps = new()
                {
                    {"$$[[FLAGBASE]]$$", GCSchemeHandler.FlagBaseCSS },

                    {"$$[[FLAGCODEPREFIX]]$$", GCSchemeHandler.FlagCodeCssClassPrefix }
                };

                reps.ForEach(pair => MapHTMLTemplate = MapHTMLTemplate.ReplaceDefault(pair.Key, pair.Value));
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
            }
        }

        private void ScoreFormulator_Load(object sender, EventArgs e)
        {

            // CREATE CONTROL
            TextArea = new Scintilla();
            TextPanel.Controls.Add(TextArea);

            // BASIC CONFIG
            TextArea.Dock = DockStyle.Fill;
            TextArea.TextChanged += OnTextChanged;

            // INITIAL VIEW CONFIG
            TextArea.WrapMode = WrapMode.None;
            TextArea.IndentationGuides = IndentView.LookBoth;

            // STYLING
            InitColors();
            InitSyntaxColoring();

            // NUMBER MARGIN
            InitNumberMargin();

            // BOOKMARK MARGIN
            InitBookmarkMargin();

            // CODE FOLDING MARGIN
            InitCodeFolding();

            // DRAG DROP
            InitDragDropFile();

            // INIT HOTKEYS
            InitHotkeys();

            TextArea.Text = LastCompiledExpression;
            numericUpDown1.Value = ScoreFormulatorExpression.PerfectScoreStatic;

            ChangesMade = false;

            // Webview
            string mapHtml = MapHTMLTemplate;

            // Create a scheduler that uses one thread.
            GCTaskScheduler lcts = new(1);

            // Create a TaskFactory and pass it our custom scheduler.
            TaskFactory factory = new(lcts);
            using CancellationTokenSource cts = new(7500);

            cts.Token.Register(() => MessageBox.Show("Please restart the Score Formulator window, map provider seem to have issues.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly));

            try
            {
                ControlBox = false;
                testMap.EnsureCoreWebView2Async()
                    .ContinueWith((_) => testMap.InvokeDefault(async () =>
                    {
                        try
                        {
                            testMap.NavigateToString(mapHtml);
                            string scr = $"(() => {{var styl = document.createElement('style');styl.innerHTML = `{GCSchemeHandler.FlagCache}`;setTimeout(() => {{console.log('adding');document.head.appendChild(styl);}},2500);}})();";
                            await testMap.ExecuteScriptAsync(scr);
                        }
                        catch (Exception ex)
                        {
                            logger.Error(ex.Summarize());
                            cts.Cancel();
                        }
                        finally
                        {
                            ControlBox = true;
                        }
                    }), cts.Token, TaskContinuationOptions.PreferFairness, lcts);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
                ControlBox = true;
            }
        }

        private void InsertMethodStringToExpression(string method, params string[] args)
        {
            InsertStringToExpression($"{method}({string.Join(", ", args)})");
        }

        private void InsertStringToExpression(string str)
        {
            int select = TextArea.SelectionStart;
            TextArea.Text = TextArea.Text.Insert(TextArea.SelectionStart, str);

            TextArea.Focus();
            TextArea.SelectionStart = select + str.Length;
            TextArea.SelectionEnd = TextArea.SelectionStart;
            TextArea.ScrollCaret();
        }

        private static string WrapExpression(string exp)
        {
            return $"return ({exp});";
        }

        /// <summary>
        /// Calculate markers' guess data on webview
        /// </summary>
        /// <returns></returns>
        public async Task CalculateMarkerData()
        {
            string str = await testMap.ExecuteScriptAsync($"(() => window.markers.map(m => m.getLatLng().lat.toString().replace(', ','.') + '&' + m.getLatLng().lng.toString().replace(', ','.')).join(' '))();")
                ;
            str = str.Trim('"').Trim();

            string correct = await testMap.ExecuteScriptAsync($"(() => window.correctMarker.getLatLng().lat.toString().replace(', ','.') + '&' + window.correctMarker.getLatLng().lng.toString().replace(', ','.'))();")
                ;
            correct = correct.Trim('"').Trim();

            string[] array = str.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string[] c = correct.Split('&', StringSplitOptions.RemoveEmptyEntries);

            string correctlat = c[0];
            string correctlng = c[1];

            List<string> data = new();

            string code = BorderHelper.GetFeatureHitBy(new double[] { correctlng.ParseAsDouble(), correctlat.ParseAsDouble() }, out GeoJson geo, out Feature hitFeature, out Polygon _);
            Country correctcountry = CountryHelper.GetCountryInformation(code, geo, hitFeature, Settings.Default.UseEnglishCountryNames, out Country correctexactcountry);

            await testMap.ExecuteScriptAsync($"(() => {{window.correctMarker.data = {$"{{exactcountryCode: '{correctexactcountry.Code}', exactcountryName: '{correctexactcountry.Name}', countryCode: '{correctcountry.Code}', countryName: '{correctcountry.Name}'}}"};return 0;}})();")
                ;

            for (int i = 0; i < array.Length; i++)
            {
                string latlng = array[i];
                string[] pair = latlng.Split("&");

                string lat = pair[0];
                string lng = pair[1];

                // TODO: Scale customizable, currently ADW scale
                ScoreFormulatorVariable scale = ScoreFormulatorExpression.MapScaleADW;
                double dist = GameHelper.HaversineDistance(
                    new GGMin() { lat = correctlat.ParseAsDouble(), lng = correctlng.ParseAsDouble() },
                    new GGMax() { lat = lat.ParseAsDouble(), lng = lng.ParseAsDouble() }
                    );
                double def = GameHelper.CalculateDefaultScore(dist, scale);

                code = BorderHelper.GetFeatureHitBy(new double[] { lng.ParseAsDouble(), lat.ParseAsDouble() }, out geo, out hitFeature, out Polygon _);

                Country country = CountryHelper.GetCountryInformation(code, geo, hitFeature, Settings.Default.UseEnglishCountryNames, out Country exactcountry);

                ScoreFormulatorExpression e = new(correctlat, correctlng, lat, lng, def.ToStringDefault(), (dist * 1000).ToStringDefault(), CurrentExpression, correctcountry, country, correctexactcountry, exactcountry);

                object res = (await LastCompiledScript.RunAsync(e)).ReturnValue;

                if (ScoreFormulatorVariable.GetAsDouble(res, out double d))
                {
                    data.Add($"{{score: '{d.ToStringDefault("F4")}', distance: '{dist.ToStringDefault("F4")}', countryCode: '{country.Code}', countryName: '{country.Name}', exactcountryCode: '{exactcountry.Code}', exactcountryName: '{exactcountry.Name}'}}");
                }
            }
            await testMap.ExecuteScriptAsync($"(() => {{var lis = [{string.Join(",", data)}]; window.markers.forEach((m,i) => m.data = lis[i]);return 0;}})();")
                ;
        }

        /// <summary>
        /// Evaluate given expression, run <see cref="EvaluateExpressionAndMarkers(string)"/> and return the test result. Returns <see cref="double.NaN"/> if unsuccesful.
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<double> EvaluateExpressionAndMarkers(string exp)
        {
            double d = EvaluateExpression(exp);
            if (!double.IsNaN(d))
            {
                string txt = calculateButton.Text;
                calculateButton.Text = "Calculating guesses on the map...";
                await CalculateMarkerData();
                calculateButton.Text = txt;
            }
            return d;
        }

        /// <summary>
        /// Evaluate given expression and return test result.
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public double EvaluateExpression(string exp)
        {
            string txt = calculateButton.Text;
            exp ??= string.Empty;
            try
            {
                ScriptOptions options = null;

                calculateButton.Text = "Initializing...";

                string trimmed = exp.Trim();
                exp = WrapExpression(trimmed);

                if (LastCompiledExpression != trimmed)
                {
                    LastCompiledExpression = trimmed;

                    LastCompiledScript = CSharpScript.Create(exp, options, CurrentExpression.GetType());

                    calculateButton.Text = "Compiling...";
                    System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic> compiled = LastCompiledScript.Compile();

                    if (compiled.Length > 0)
                    {
                        calculateButton.Text = "Invalid expression!";
                        MessageBox.Show("Expression failed to compile with following errors:\n" + string.Join("\n", compiled), "Compile Error");
                        return double.NaN;
                    }
                }

                calculateButton.Text = "Evaluating...";
                object res = LastCompiledScript.RunAsync(CurrentExpression).Result.ReturnValue;

                if (ScoreFormulatorVariable.GetAsDouble(res, out double d))
                {
                    ChangesMade = false;
                    Settings.Default.ScoreFormula = LastCompiledExpression;
                    return d;
                }

                MessageBox.Show("Expression has to return a numeric value! Got type: " + res?.GetType().GetFriendlyName(), "Unexpected Return Type");
                return double.NaN;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Expression Error");
                return double.NaN;
            }
            finally
            {
                calculateButton.Text = txt;
            }
        }

        private void testVariablesDataGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
            e.Cancel = true;
            object newval = ((DataGridView)sender).CurrentRow.Cells[1].EditedFormattedValue;
            if (ScoreFormulatorVariable.GetAsDouble(newval, out double val))
            {
                datatable.Rows[e.RowIndex][e.ColumnIndex] = new ScoreFormulatorVariable(val);
                CurrentExpression.GetType().GetProperty(datatable.Rows[e.RowIndex][0].ToStringDefault()).SetValue(CurrentExpression, datatable.Rows[e.RowIndex][e.ColumnIndex]);
            }
            else
            {
                MessageBox.Show("Invalid value type. Expected numeric value", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private async void calculateButton_Click(object sender, EventArgs e)
        {
            double res = await EvaluateExpressionAndMarkers(TextArea.Text);
            testResultLabel.Text = double.IsNaN(res)
                ? "Invalid"
                : res.ToStringDefault("F4");
        }

#region INSERT BUTTONS
        private void button21_Click(object sender, EventArgs e)
        {
            InsertStringToExpression("+");
        }

        private void button16_Click(object sender, EventArgs e)
        {
            InsertStringToExpression("-");
        }

        private void button23_Click(object sender, EventArgs e)
        {
            InsertStringToExpression("*");
        }

        private void button25_Click(object sender, EventArgs e)
        {
            InsertStringToExpression("/");
        }

        private void button26_Click(object sender, EventArgs e)
        {
            InsertStringToExpression("%");
        }

        private void button30_Click(object sender, EventArgs e)
        {
            InsertStringToExpression("(");
        }

        private void button29_Click(object sender, EventArgs e)
        {
            InsertStringToExpression(",");
        }

        private void button28_Click(object sender, EventArgs e)
        {
            InsertStringToExpression(")");
        }

        private void button32_Click(object sender, EventArgs e)
        {
            InsertStringToExpression("<");
        }

        private void button31_Click(object sender, EventArgs e)
        {
            InsertStringToExpression("<=");
        }

        private void button27_Click(object sender, EventArgs e)
        {
            InsertStringToExpression(">");
        }

        private void button24_Click(object sender, EventArgs e)
        {
            InsertStringToExpression(">=");
        }

        private void button34_Click(object sender, EventArgs e)
        {
            InsertStringToExpression("==");
        }

        private void button36_Click(object sender, EventArgs e)
        {
            InsertStringToExpression("!=");
        }

        private void button37_Click(object sender, EventArgs e)
        {
            InsertStringToExpression("&&");
        }

        private void button33_Click(object sender, EventArgs e)
        {
            InsertStringToExpression("||");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            InsertStringToExpression(nameof(ScoreFormulatorExpression.GuessLatitude));
        }

        private void button8_Click(object sender, EventArgs e)
        {
            InsertStringToExpression(nameof(ScoreFormulatorExpression.GuessLongitude));
        }

        private void button9_Click(object sender, EventArgs e)
        {
            InsertStringToExpression(nameof(ScoreFormulatorExpression.CorrectLatitude));
        }

        private void button7_Click(object sender, EventArgs e)
        {
            InsertStringToExpression(nameof(ScoreFormulatorExpression.CorrectLongitude));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            InsertStringToExpression(nameof(ScoreFormulatorExpression.DistanceMeters));
        }
        private void button10_Click(object sender, EventArgs e)
        {
            InsertStringToExpression(nameof(ScoreFormulatorExpression.DistanceFeet));
        }
        private void button41_Click(object sender, EventArgs e)
        {
            InsertStringToExpression(nameof(ScoreFormulatorExpression.DistanceKilometers));
        }
        private void button42_Click(object sender, EventArgs e)
        {
            InsertStringToExpression(nameof(ScoreFormulatorExpression.DistanceMiles));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            InsertStringToExpression(nameof(ScoreFormulatorExpression.Time));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            InsertStringToExpression(nameof(ScoreFormulatorExpression.Streak));
        }


        private void button5_Click(object sender, EventArgs e)
        {
            InsertStringToExpression(nameof(ScoreFormulatorExpression.GuesserNumber));
        }

        private void button11_Click(object sender, EventArgs e)
        {
            InsertStringToExpression(nameof(ScoreFormulatorExpression.GuessesMade));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            InsertStringToExpression(nameof(ScoreFormulatorExpression.RoundNumber));
        }

        private void button22_Click(object sender, EventArgs e)
        {
            InsertStringToExpression(nameof(ScoreFormulatorExpression.DefaultScore));
        }

        private void button39_Click(object sender, EventArgs e)
        {
            InsertStringToExpression(nameof(ScoreFormulatorExpression.IsCorrectCountry));
        }

        private void button40_Click(object sender, EventArgs e)
        {
            InsertStringToExpression(nameof(ScoreFormulatorExpression.IsCorrectRegion));
        }

        private void button46_Click(object sender, EventArgs e)
        {
            InsertStringToExpression(nameof(ScoreFormulatorExpression.IsRandomGuess));
        }

        private void button47_Click(object sender, EventArgs e)
        {
            InsertStringToExpression(nameof(ScoreFormulatorExpression.PerfectScore));
        }

        private void button48_Click(object sender, EventArgs e)
        {
            InsertStringToExpression(nameof(ScoreFormulatorExpression.MapScale));
        }

        private void button12_Click(object sender, EventArgs e)
        {
            InsertMethodStringToExpression(nameof(ScoreFormulatorExpression.Case), "condition", "successValue", "failValue");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            InsertMethodStringToExpression(nameof(ScoreFormulatorExpression.Min), "value1", "value2");
        }

        private void button19_Click(object sender, EventArgs e)
        {
            InsertMethodStringToExpression(nameof(ScoreFormulatorExpression.Max), "value1", "value2");
        }

        private void button17_Click(object sender, EventArgs e)
        {
            InsertMethodStringToExpression(nameof(ScoreFormulatorExpression.Round), "value", "digits");
        }

        private void button18_Click(object sender, EventArgs e)
        {
            InsertMethodStringToExpression(nameof(ScoreFormulatorExpression.Random), "min", "max");
        }

        private void button14_Click(object sender, EventArgs e)
        {
            InsertMethodStringToExpression(nameof(ScoreFormulatorExpression.RandomPick), "value1", "value2");
        }

        private void button35_Click(object sender, EventArgs e)
        {
            InsertMethodStringToExpression(nameof(ScoreFormulatorExpression.Pow), "baseNumber", "power");
        }

        private void button38_Click(object sender, EventArgs e)
        {
            InsertMethodStringToExpression(nameof(ScoreFormulatorExpression.Abs), "value");
        }

#endregion

#region Text Editor

        private void InitColors()
        {
            TextArea.SetSelectionBackColor(true, IntToColor(0x114D9C));
        }

        private void InitHotkeys()
        {
            // register the hotkeys with the form
            HotKeyManager.AddHotKey(this, Uppercase, Keys.U, true);
            HotKeyManager.AddHotKey(this, Lowercase, Keys.L, true);
            HotKeyManager.AddHotKey(this, ZoomIn, Keys.Oemplus, true);
            HotKeyManager.AddHotKey(this, ZoomOut, Keys.OemMinus, true);
            HotKeyManager.AddHotKey(this, ZoomDefault, Keys.D0, true);

            // remove conflicting hotkeys from scintilla
            TextArea.ClearCmdKey(Keys.Control | Keys.F);
            TextArea.ClearCmdKey(Keys.Control | Keys.R);
            TextArea.ClearCmdKey(Keys.Control | Keys.H);
            TextArea.ClearCmdKey(Keys.Control | Keys.L);
            TextArea.ClearCmdKey(Keys.Control | Keys.U);
            TextArea.ClearCmdKey(Keys.Control | Keys.E);
            TextArea.ClearCmdKey(Keys.Control | Keys.Z);

        }

        private void InitSyntaxColoring()
        {

            TextArea.StyleResetDefault();
            TextArea.Styles[Style.Default].Font = "Consolas";
            TextArea.Styles[Style.Default].Size = 10;
            TextArea.Styles[Style.Default].BackColor = IntToColor(0x212121);
            TextArea.Styles[Style.Default].ForeColor = IntToColor(0xFFFFFF);
            TextArea.StyleClearAll();

            TextArea.Styles[Style.Cpp.Identifier].ForeColor = IntToColor(0xD0DAE2);
            TextArea.Styles[Style.Cpp.Comment].ForeColor = IntToColor(0xBD758B);
            TextArea.Styles[Style.Cpp.CommentLine].ForeColor = IntToColor(0x40BF57);
            TextArea.Styles[Style.Cpp.CommentDoc].ForeColor = IntToColor(0x2FAE35);
            TextArea.Styles[Style.Cpp.Number].ForeColor = IntToColor(0xFFFF00);
            TextArea.Styles[Style.Cpp.String].ForeColor = IntToColor(0xFFFF00);
            TextArea.Styles[Style.Cpp.Character].ForeColor = IntToColor(0xE95454);
            TextArea.Styles[Style.Cpp.Preprocessor].ForeColor = IntToColor(0x8AAFEE);
            TextArea.Styles[Style.Cpp.Operator].ForeColor = IntToColor(0xE0E0E0);
            TextArea.Styles[Style.Cpp.Regex].ForeColor = IntToColor(0xff00ff);
            TextArea.Styles[Style.Cpp.CommentLineDoc].ForeColor = IntToColor(0x77A7DB);
            TextArea.Styles[Style.Cpp.Word].ForeColor = IntToColor(0x48A8EE);
            TextArea.Styles[Style.Cpp.Word2].ForeColor = IntToColor(0xF98906);
            TextArea.Styles[Style.Cpp.CommentDocKeyword].ForeColor = IntToColor(0xB3D991);
            TextArea.Styles[Style.Cpp.CommentDocKeywordError].ForeColor = IntToColor(0xFF0000);
            TextArea.Styles[Style.Cpp.GlobalClass].ForeColor = IntToColor(0x48A8EE);

            TextArea.LexerName = "cpp";

            TextArea.SetKeywords(1, "add and alias ascending args async await by descending dynamic equals from get global group init into join let managed nameof nint not notnull nuint on or orderby partial partial record remove select set unmanaged unmanaged value var when where where with yield"
                + " " + string.Join(' ', typeof(ScoreFormulatorExpression).GetProperties(BindingFlags.Instance | BindingFlags.Public).Select(p => p.Name)));
            TextArea.SetKeywords(0, "abstract as base bool break byte case catch char checked class const continue decimal default delegate do double else enum event explicit extern false finally fixed float for foreach goto if implicit in int interface internal is lock long namespace new null object operator out override params private protected public readonly ref return sbyte sealed short sizeof stackalloc static string struct switch this throw true try typeof uint ulong unchecked unsafe ushort using virtual void volatile while"
                + " " + string.Join(' ', typeof(ScoreFormulatorExpression).GetMethods(BindingFlags.Static | BindingFlags.Public).Where(m => !m.IsSpecialName).Select(p => p.Name).Distinct()));
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            ChangesMade = LastCompiledExpression != TextArea.Text;
        }

#region Numbers, Bookmarks, Code Folding

        /// <summary>
        /// the background color of the text area
        /// </summary>
        private const int BACK_COLOR = 0x3A312C;

        /// <summary>
        /// default text color of the text area
        /// </summary>
        private const int FORE_COLOR = 0xB7B7B7;

        /// <summary>
        /// change this to whatever margin you want the line numbers to show in
        /// </summary>
        private const int NUMBER_MARGIN = 1;

        /// <summary>
        /// change this to whatever margin you want the bookmarks/breakpoints to show in
        /// </summary>
        private const int BOOKMARK_MARGIN = 2;
        private const int BOOKMARK_MARKER = 2;

        /// <summary>
        /// change this to whatever margin you want the code folding tree (+/-) to show in
        /// </summary>
        private const int FOLDING_MARGIN = 3;

        /// <summary>
        /// set this true to show circular buttons for code folding (the [+] and [-] buttons on the margin)
        /// </summary>
        private const bool CODEFOLDING_CIRCULAR = true;

        private void InitNumberMargin()
        {
            TextArea.CaretForeColor = Color.White;
            TextArea.CaretWidth = 2;

            TextArea.Styles[Style.LineNumber].BackColor = IntToColor(BACK_COLOR);
            TextArea.Styles[Style.LineNumber].ForeColor = IntToColor(FORE_COLOR);
            TextArea.Styles[Style.IndentGuide].ForeColor = IntToColor(FORE_COLOR);
            TextArea.Styles[Style.IndentGuide].BackColor = IntToColor(BACK_COLOR);

            Margin nums = TextArea.Margins[NUMBER_MARGIN];
            nums.Width = 12;
            nums.Type = MarginType.Number;
            nums.Sensitive = true;
            nums.Mask = 0;

            TextArea.MarginClick += TextArea_MarginClick;
        }

        private void InitBookmarkMargin()
        {

            //TextArea.SetFoldMarginColor(true, IntToColor(BACK_COLOR));

            Margin margin = TextArea.Margins[BOOKMARK_MARGIN];
            margin.Width = 12;
            margin.Sensitive = true;
            margin.Type = MarginType.Symbol;
            margin.Mask = 1 << BOOKMARK_MARKER;
            //margin.Cursor = MarginCursor.Arrow;

            Marker marker = TextArea.Markers[BOOKMARK_MARKER];
            marker.Symbol = MarkerSymbol.Bookmark;
            marker.SetBackColor(IntToColor(0xFF003B));
            marker.SetForeColor(IntToColor(0x000000));
            marker.SetAlpha(100);

        }

        private void InitCodeFolding()
        {

            TextArea.SetFoldMarginColor(true, IntToColor(BACK_COLOR));
            TextArea.SetFoldMarginHighlightColor(true, IntToColor(BACK_COLOR));

            // Enable code folding
            TextArea.SetProperty("fold", "1");
            TextArea.SetProperty("fold.compact", "1");

            // Configure a margin to display folding symbols
            TextArea.Margins[FOLDING_MARGIN].Type = MarginType.Symbol;
            TextArea.Margins[FOLDING_MARGIN].Mask = Marker.MaskFolders;
            TextArea.Margins[FOLDING_MARGIN].Sensitive = true;
            TextArea.Margins[FOLDING_MARGIN].Width = 20;

            // Set colors for all folding markers
            for (int i = 25; i <= 31; i++)
            {
                TextArea.Markers[i].SetForeColor(IntToColor(BACK_COLOR)); // styles for [+] and [-]
                TextArea.Markers[i].SetBackColor(IntToColor(FORE_COLOR)); // styles for [+] and [-]
            }

            // Configure folding markers with respective symbols
            TextArea.Markers[Marker.Folder].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CirclePlus : MarkerSymbol.BoxPlus;
            TextArea.Markers[Marker.FolderOpen].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CircleMinus : MarkerSymbol.BoxMinus;
            TextArea.Markers[Marker.FolderEnd].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CirclePlusConnected : MarkerSymbol.BoxPlusConnected;
            TextArea.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
            TextArea.Markers[Marker.FolderOpenMid].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CircleMinusConnected : MarkerSymbol.BoxMinusConnected;
            TextArea.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
            TextArea.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

            // Enable automatic folding
            TextArea.AutomaticFold = AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change;

        }

        private void TextArea_MarginClick(object sender, MarginClickEventArgs e)
        {
            if (e.Margin == BOOKMARK_MARGIN)
            {
                // Do we have a marker for this line?
                const uint mask = 1 << BOOKMARK_MARKER;
                Line line = TextArea.Lines[TextArea.LineFromPosition(e.Position)];
                if ((line.MarkerGet() & mask) > 0)
                {
                    // Remove existing bookmark
                    line.MarkerDelete(BOOKMARK_MARKER);
                }
                else
                {
                    // Add bookmark
                    line.MarkerAdd(BOOKMARK_MARKER);
                }
            }
        }

#endregion

#region Drag & Drop File

        private void InitDragDropFile()
        {

            TextArea.AllowDrop = true;
            TextArea.DragEnter += delegate (object sender, DragEventArgs e)
            {
                e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
            };
            TextArea.DragDrop += delegate (object sender, DragEventArgs e)
            {

                // get file drop
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {

                    Array a = (Array)e.Data.GetData(DataFormats.FileDrop);
                    if (a != null)
                    {

                        string path = a.GetValue(0).ToString();

                        LoadDataFromFile(path);

                    }
                }
            };

        }

        private void LoadDataFromFile(string path)
        {
            if (File.Exists(path))
            {
                TextArea.Text = File.ReadAllText(path);
            }
        }
#endregion

#region Main Menu Commands

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextArea.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextArea.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextArea.Paste();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextArea.SelectAll();
        }

        private void selectLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Line line = TextArea.Lines[TextArea.CurrentLine];
            TextArea.SetSelection(line.Position + line.Length, line.Position);
        }

        private void clearSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextArea.SetEmptySelection(0);
        }

        private void indentSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Indent();
        }

        private void outdentSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Outdent();
        }

        private void uppercaseSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Uppercase();
        }

        private void lowercaseSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Lowercase();
        }

        private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZoomIn();
        }

        private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZoomOut();
        }

        private void zoom100ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZoomDefault();
        }

        private void collapseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextArea.FoldAll(FoldAction.Contract);
        }

        private void expandAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextArea.FoldAll(FoldAction.Expand);
        }


#endregion

#region Uppercase / Lowercase

        private void Lowercase()
        {

            // save the selection
            int start = TextArea.SelectionStart;
            int end = TextArea.SelectionEnd;

            // modify the selected text
            TextArea.ReplaceSelection(TextArea.GetTextRange(start, end - start).ToLowerInvariant());

            // preserve the original selection
            TextArea.SetSelection(start, end);
        }

        private void Uppercase()
        {

            // save the selection
            int start = TextArea.SelectionStart;
            int end = TextArea.SelectionEnd;

            // modify the selected text
            TextArea.ReplaceSelection(TextArea.GetTextRange(start, end - start).ToUpperInvariant());

            // preserve the original selection
            TextArea.SetSelection(start, end);
        }

#endregion

#region Indent / Outdent

        private void Indent()
        {
            // we use this hack to send "Shift+Tab" to scintilla, since there is no known API to indent,
            // although the indentation function exists. Pressing TAB with the editor focused confirms this.
            GenerateKeystrokes("{TAB}");
        }

        private void Outdent()
        {
            // we use this hack to send "Shift+Tab" to scintilla, since there is no known API to outdent,
            // although the indentation function exists. Pressing Shift+Tab with the editor focused confirms this.
            GenerateKeystrokes("+{TAB}");
        }

        private void GenerateKeystrokes(string keys)
        {
            HotKeyManager.Enable = false;
            TextArea.Focus();
            SendKeys.Send(keys);
            HotKeyManager.Enable = true;
        }

#endregion

#region Zoom

        private void ZoomIn()
        {
            TextArea.ZoomIn();
        }

        private void ZoomOut()
        {
            TextArea.ZoomOut();
        }

        private void ZoomDefault()
        {
            TextArea.Zoom = 0;
        }


#endregion

#region Utils

        private static Color IntToColor(int rgb)
        {
            return Color.FromArgb(255, (byte)(rgb >> 16), (byte)(rgb >> 8), (byte)rgb);
        }

        private void InvokeIfNeeded(Action action)
        {
            if (InvokeRequired)
            {
                BeginInvoke(action);
            }
            else
            {
                action?.Invoke();
            }
        }




#endregion

#endregion

        private int currentTestMarkerMode = 1;
        private async void button43_Click(object sender, EventArgs e)
        {
            currentTestMarkerMode ^= 1;
            await testMap.ExecuteScriptAsync("(() => window.clickMode=" + currentTestMarkerMode + ")();");

            button43.Text = currentTestMarkerMode == 1 ? "Add" : "Remove";
        }

        private RandomNumberGenerator random = RandomNumberGenerator.Create();

        private async void button45_Click(object sender, EventArgs e)
        {
            Coordinates rand = BorderHelper.GetRandomPointCloseOrWithinAPolygon();

            await testMap.ExecuteScriptAsync($"(() => window.addMarker(L.latLng({rand.Latitude.ToStringDefault()},{rand.Longitude.ToStringDefault()})))();");
        }

        private async void button44_Click(object sender, EventArgs e)
        {
            await testMap.ExecuteScriptAsync($"(() => {{console.log(window.markers);while(window.markers[0]){{var m = window.markers.pop(); if(m) m.remove();}}}})();");
        }

        private class CustomFormula
        {
            private string code;

            public string Code
            {
                get => code;
                set
                {
                    code = value;
                    Script = CSharpScript.Create(WrapExpression(code), null, CurrentExpression.GetType());
                }
            }

            public string Description { get; set; }

            public Script<object> Script { get; set; }

            public CustomFormula(string info, string code)
            {
                Description = info;
                Code = code;
            }
        }

        private static long PerfectScoreForFurthest { get; } = 17000;

        private static Dictionary<string, CustomFormula> Formulas { get; } = new Dictionary<string, CustomFormula>()
        {
            { "Default", new("Default score calculation", nameof(ScoreFormulatorExpression.DefaultScore)) },
            //{ "Worst is Best!", new($"Furthest away gets more points!", $"{nameof(ScoreFormulatorExpression.Case)}({nameof(ScoreFormulatorExpression.DistanceKilometers)} >= {PerfectScoreForFurthest}, {nameof(ScoreFormulatorExpression.PerfectScore)}, {nameof(ScoreFormulatorExpression.Round)}({nameof(ScoreFormulatorExpression.PerfectScore)} * {nameof(ScoreFormulatorExpression.Pow)}(0.99866017, ({PerfectScoreForFurthest} - {nameof(ScoreFormulatorExpression.DistanceKilometers)}) * 1000 / {nameof(ScoreFormulatorExpression.MapScale)})))") },
            { "LatitudeGuessr", new("Get within 1 degrees of difference in latitude for more points!", $"{nameof(ScoreFormulatorExpression.Min)}({nameof(ScoreFormulatorExpression.PerfectScore)}, {nameof(ScoreFormulatorExpression.DefaultScore)} * {nameof(ScoreFormulatorExpression.Case)}({nameof(ScoreFormulatorExpression.Abs)}({nameof(ScoreFormulatorExpression.GuessLatitude)} - {nameof(ScoreFormulatorExpression.CorrectLatitude)}) < 1, 1, 0.5) )") },
            { "Country or Bust!", new("Only the guesses in correct country are calculated, rest get 0.", $"{nameof(ScoreFormulatorExpression.Case)}({nameof(ScoreFormulatorExpression.IsCorrectCountry)}, {nameof(ScoreFormulatorExpression.DefaultScore)}, 0)") },
            { "Region or Bust!", new("Only the guesses in correct region are calculated, rest get 0.", $"{nameof(ScoreFormulatorExpression.Case)}({nameof(ScoreFormulatorExpression.IsCorrectRegion)}, {nameof(ScoreFormulatorExpression.DefaultScore)}, 0)") },
        };

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string item = ((ComboBox)sender).SelectedItem?.ToStringDefault()?.Trim();
            if (Formulas.ContainsKey(item))
            {
                CustomFormula i = Formulas[item];
                TextArea.Text = i.Code;
                LastCompiledExpression = i.Code;
                Settings.Default.ScoreFormula = LastCompiledExpression;
                LastCompiledScript = i.Script;
                ChangesMade = false;
                MessageBox.Show($"Applied new formula '{item}': {i.Description}", "Score Formulation Changed", MessageBoxButtons.OK);
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            decimal v = ((NumericUpDown)sender).Value;
            ScoreFormulatorExpression.PerfectScoreStatic = new(v);
        }

        private void ScoreFormulator_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ChangesMade)
            {
                DialogResult res = MessageBox.Show("Score expression change since last compilation. Would you like to compile last changes and exit?", "Unsaved changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                switch (res)
                {
                    case DialogResult.Yes:
                        double d = EvaluateExpression(TextArea.Text);
                        e.Cancel = double.IsNaN(d);
                        ChangesMade = false;
                        return;
                    case DialogResult.No:
                        return;
                    default:
                        e.Cancel = true;
                        return;
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ScoreFormulator_FormClosed(object sender, FormClosedEventArgs e)
        {
            TextArea?.Dispose();
            random?.Dispose();
        }

        private void button43_Enter(object sender, EventArgs e)
        {

        }

        private void ScoreFormulator_Enter(object sender, EventArgs e)
        {
            mainForm.ResetJSCTRLCheck();
        }
    }
}
