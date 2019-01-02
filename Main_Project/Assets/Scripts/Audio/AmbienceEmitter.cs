
public class AmbienceEmitter : AudioEmitter {


    public override void Start() {

        base.Start();
        source.volume = AudioManager.Instance.volumeAmbience;
        

        AudioManager.Instance.ambienceSourceList.Add(source);
    }


}
