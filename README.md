# ettessaCHelper: Better Cassette Music
## Version 1.0
### About
ettessaCHelper provides a better experience when trying to import your music of choice into Celeste to be used as a cassette song. The default cassette music system is not optimized for full songs and can cause music to sound very glitchy when importing a song, however ettessaCHelper rewrites this system to sound better when importing whole songs to be used as cassette music.

### Disclaimer
This is my first code mod. I made it specifically for one of my solo projects. It still has a tendency to desync after a while or with excessive pausing.

### Use

1. Get a .wav file of your song
2. Follow [the guide for cassette music](https://github.com/EverestAPI/Resources/wiki/Advanced-Custom-Audio#adding-cassette-music) on the Everest Wiki, skipping steps 3-7
3. Create a new parameter, named "autosync", with a max value 1 more than the value of "Max Beats" in Loenn.
4. Copy-paste your .wav file into the track of the "autosync" parameter, and scale it to fill the entire track.
5. Select the sample, and expand the external sample editor so that the "starting position" knob is visible.
6. Right click on the knob and select "Add Automation", and add 2 points at each end from 0 to whatever your maximum value is.
7. Now changing the value of "autosync" should change where playback starts when you hit play.
8. Build your bank just like you would normally, and then set the cassette music in Loenn to your cassette song, and you should be good to go!


