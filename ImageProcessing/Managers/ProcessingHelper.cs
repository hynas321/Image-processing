namespace Image_processing.Managers
{
    public class ProcessingHelper
    {

        public static bool GetPhasePreservationInput(char key)
        {
            if (key == 'y' || key == 'Y')
            {
                return true;
            }

            return false;
        }
    }
}
