using System;
using System.Collections.Generic;
using SDL2;
using SDL2.SDL;

namespace Gui
{
    public class SdlAudio
    {
        public List<SdlAudioDevice> GetPlaybackDevies()
        {
            const int requestPlaybackDevices = 0;
            var numberOfAudioDevices = SDL.SDL_GetNumAudioDevices(requestPlaybackDevices);
            var audioDevies = new List<SdlAudioDevice>();
            for (var i = 0; i < numberOfAudioDevices; i++)
            {
                audioDevies.Add(new SdlAudioDevice(SDL.SDL_GetAudioDeviceName(i, requestPlaybackDevices)));
            }

            return audioDevies;
        }

        public class SdlAudioDevice
        {
            private readonly string _deviceName;
            private SDL_AudioCallback _callback;

            public SdlAudioDevice(string deviceName)
            {
                _deviceName = deviceName;
            }

            public void Open()
            {
                _callback = new SDL_AudioCallback(Callback);
                var wanted = new SDL_AudioSpec
                {
                    freq = 48000,
                    format = AUDIO_F32,
                    channels = 2,
                    samples = 4096,
                    callback = _callback,
                };
                SDL_AudioSpec obtained;
                var result = SDL.SDL_OpenAudioDevice(_deviceName, 0, ref wanted, out obtained, (int)SDL_AUDIO_ALLOW_ANY_CHANGE);
                SDL.SDL_PauseAudioDevice(result, 0);
            }

            private int offset;
            private void Callback(IntPtr userdata, IntPtr stream, int len)
            {
                unsafe
                {
                    var waveFrequency = (Math.PI * 2) * 1000.0;
                    var frequency = 48000.0;

                    var pointer = stream.ToPointer();
                    var fPointer = (float*)pointer;

                    var step = waveFrequency/frequency;

                    var samples = len / (2 * sizeof(float));
                    for (int sample = 0; sample < samples; sample++)
                    {
                        for (int channel = 0; channel < 2; channel++)
                        {
                            var data = Math.Sin((offset + sample) * step);
                            *(fPointer + sample * 2 + channel) = (float)data;
                        }
                    }

                    offset+=samples;
                }
            }
        }
    }
}