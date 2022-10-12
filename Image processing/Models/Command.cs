namespace Image_processing.Models
{
    public class Command
    {
        public string FileName { get; set; }
        public string Operation { get; set; }
        public string ArgumentValue { get; set; }

        public Command(
            string filename,
            string operation,
            string argumentValue
        )
        {
            FileName = filename;
            Operation = operation;
            ArgumentValue = argumentValue;
        }

        public override string ToString()
        {
            if (FileName == null)
            {
                return $"null";
            }
            if (Operation == null)
            {
                return $"{FileName}";
            }
            if (ArgumentValue == null)
            {
                return $"{FileName} {Operation}";
            }

            return $"{FileName} {Operation} {ArgumentValue}";
        }
    }
}
