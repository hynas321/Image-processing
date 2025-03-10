namespace Image_processing.Helpers
{
    public class ProcessingHelper
    {
        public static bool GetPhasePreservationInput(char key)
        {
            return key == 'y' || key == 'Y';
        }
    }
}
