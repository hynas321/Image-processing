﻿namespace Image_processing.Managers
{
    public class ProcessingHelper
    {
        public static bool GetPhasePreservationInput(char key)
        {
            return key == 'y' || key == 'Y';
        }
    }
}
