namespace ExtractNameFromMail_1
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using Skyline.DataMiner.Analytics.GenericInterface;
    using Skyline.DataMiner.Net;
    using Skyline.DataMiner.Net.Helper;

    [GQIMetaData(Name = "GQIO - RBM - RegexMatchGroup 1.3")]
    public class ExtractStringWithRegex : IGQIInputArguments, IGQIColumnOperator, IGQIRowOperator
    {
        private GQIColumnDropdownArgument _firstColumnArg = new GQIColumnDropdownArgument("Input column") { IsRequired = true, Types = new GQIColumnType[] { GQIColumnType.String } };
        private GQIStringArgument _regexArg = new GQIStringArgument("RegEx") { IsRequired = true };
        private GQIDoubleArgument _regexMatchArg = new GQIDoubleArgument("Match Group") { IsRequired = true, DefaultValue = 0 };
        private GQIStringArgument _nameArg = new GQIStringArgument("Column name") { IsRequired = true };

        private GQIColumn _inputColumn;
        private GQIStringColumn _newColumn;
        private string _regex;
        private double _matchGroup;

        public GQIArgument[] GetInputArguments()
        {
            return new GQIArgument[] { _firstColumnArg, _regexArg,_regexMatchArg, _nameArg };
        }



        public void HandleColumns(GQIEditableHeader header)
        {
            header.AddColumns(_newColumn);
        }

        public void HandleRow(GQIEditableRow row)
        {
            

            string inputString = Convert.ToString(row.GetValue(_inputColumn.Name));

            if (inputString.IsNotNullOrEmpty())
            {
                int matchgroup = Convert.ToInt32(_matchGroup);
                string output = GetMatchFromInput(inputString, _regex, matchgroup);
                row.SetValue(_newColumn, output);
            }
        }

        public OnArgumentsProcessedOutputArgs OnArgumentsProcessed(OnArgumentsProcessedInputArgs args)
        {
            _inputColumn = args.GetArgumentValue(_firstColumnArg); // Getting Input argument value;
            _newColumn = new GQIStringColumn(args.GetArgumentValue(_nameArg));
            _regex = args.GetArgumentValue(_regexArg);
            _matchGroup = args.GetArgumentValue(_regexMatchArg);
            return default;

        }

        private static string GetMatchFromInput(string inputString, string inputregex, int matchgroup)
        {
            string outputString;
            Regex regex = new Regex($@"{inputregex}");
            Match m = regex.Match(inputString);
            if (m.Success)
            {
                Group g = m.Groups[matchgroup];
                outputString = g.Value;
            }
            else
            {
                outputString = string.Empty;
            }
            return outputString;
        }
    }
}