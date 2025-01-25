namespace uMicrophoneWebGL {

    public class Device {
        public int index;
        public string deviceId;
        public string label;
        public int sampleRate;
        public int channelCount;

        public static Device invalid = new() {
            index = -1,
            deviceId = "",
            label = "",
            sampleRate = 0,
            channelCount = 1,
        };
    }
}