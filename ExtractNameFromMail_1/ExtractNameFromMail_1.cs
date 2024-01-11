namespace ExtractNameFromMail_1
{
    using System;
    using System.Globalization;

    using Skyline.DataMiner.Analytics.GenericInterface;

    [GQIMetaData(Name = "GQIO - ExtractNameFromMail")]
    public class ExtractNameFromMailOperator : IGQIInputArguments, IGQIColumnOperator, IGQIRowOperator
    {
        private readonly GQIColumnDropdownArgument _emailColumnArg; // Input Argument to be requested from the user;
        private readonly GQIStringColumn _nameColumn; // New Column to be added;

        private GQIColumn _emailColumn;

        public ExtractNameFromMailOperator()
        {
            _emailColumnArg = new GQIColumnDropdownArgument("Email")
            {
                IsRequired = true,
                Types = new[] { GQIColumnType.String },
            };

            _nameColumn = new GQIStringColumn("Name");
        }

        public GQIArgument[] GetInputArguments()
        {
            return new GQIArgument[] { _emailColumnArg };
        }

        public void HandleColumns(GQIEditableHeader header)
        {
            header.AddColumns(_nameColumn);
        }

        public void HandleRow(GQIEditableRow row)
        {
            string email = row.GetValue<string>(_emailColumn);
            string name = GetNameFromMail(email);

            row.SetValue(_nameColumn, name);
        }

        public OnArgumentsProcessedOutputArgs OnArgumentsProcessed(OnArgumentsProcessedInputArgs args)
        {
            _emailColumn = args.GetArgumentValue(_emailColumnArg); // Getting Input argument value;
            return default;
        }

        private static string GetNameFromMail(string email)
        {
            var partBeforeAt = email.Split('@')[0];
            var nameParts = partBeforeAt.Split('.');

            var formatedName = String.Join(" ", nameParts);
            string result = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(formatedName);

            return result;
        }
    }
}