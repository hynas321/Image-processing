namespace Image_processing.Models
{
    public class Command
    {
        public string ImageFile { get; }
        public string Operation { get; }
        public string ArgumentValue { get; }

        public Command(
            string imageFile,
            string operation,
            string argumentValue
        )
        {
            ImageFile = imageFile;
            Operation = operation;
            ArgumentValue = argumentValue;
        }

        public override string ToString()
        {
            if (ImageFile == null)
            {
                return $"null";
            }
            if (Operation == null)
            {
                return $"{ImageFile}";
            }
            if (ArgumentValue == null)
            {
                return $"{ImageFile} {Operation}";
            }

            return $"{ImageFile} {Operation} {ArgumentValue}";
        }
    }
}
