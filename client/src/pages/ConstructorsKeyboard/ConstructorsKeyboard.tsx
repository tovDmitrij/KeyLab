import { FC, useEffect, useRef, useState } from "react";

import { Canvas, useFrame, useLoader, useThree } from "@react-three/fiber";
import { useNavigate } from "react-router-dom";
import * as THREE from "three";

import Header from "../../components/Header/Header";
import { Grid } from "@mui/material";
import { GLTFExporter, GLTFLoader } from "three/examples/jsm/Addons.js";
import { OrbitControls, PerspectiveCamera, useAnimations } from "@react-three/drei";
import BoxesList from "../../components/List/ListBoxes/BoxesList";
import { useLazyGetBoxesQuery, usePostBoxMutation } from "../../services/boxesService";
import ListBoxesNew from "../../components/List/ListBoxes/ListBoxesNew"
import { useAppDispatch, useAppSelector } from "../../store/redux";
import { useGetSwitchSoundQuery, useLazyGetSwitchQuery, useLazyGetSwitchSoundQuery } from "../../services/switchesService";
import { useLazyGetKeycapQuery, useLazyGetKeycapsQuery } from "../../services/keycapsService";
import Box from "../../components/Models/Box";
import KeayboardComponents from "../../components/KeayboardComponents/KeayboardComponents";
import { Keycap } from "../../constants";
import { usePostKeyboardMutation } from "../../services/keyboardService";
import { resetKeyBoardState } from "../../store/keyboardSlice";


const Keyboard: FC<any> = ({setKeyboardModel, keycapList, boxModel, switchModel, switchSound}) => {
  const [boxScene, setBoxScene] = useState<THREE.Group<THREE.Object3DEventMap>>();
  const [pressedKey, setPressedKey] = useState('');
  const [count, setCount] = useState(0);
  const ref = useRef(null)
  const snd = new Audio("data:audio/mp3;base64," + switchSound?.soundBase64);

  let mixer: THREE.AnimationMixer;

  const handleKeyDown = (event: KeyboardEvent) => {
    event.preventDefault();
    setCount(prevCount =>  prevCount  + 1);
    setPressedKey(event.code);
  
  };   
  
  useEffect(() => {
    window.addEventListener('keydown', handleKeyDown);
    return () => {
      window.removeEventListener('keydown', handleKeyDown);
    };
  }, []);

  useEffect(() => {
    if (ref && ref.current !== null && pressedKey) {
      if (!mixer) {
        mixer = new THREE.AnimationMixer(ref.current);
      }

      const keys = Object.keys(Keycap) as Array<keyof typeof Keycap>;
      let keyName : string;
      for (let key of keys) {

        
        if (key === pressedKey) {
          keyName = Keycap[key] 
          break;
        } else {
          // console.log(pressedKey)
        }
      }

      const matchedKeycap = keycapList?.find((keyCap: any) => 
        
        keyCap.scene.children[0].name === keyName
  
      );

      if (matchedKeycap) {
        const action1 = mixer?.clipAction(matchedKeycap?.animations[0]);
        snd.play();
        action1.play();
        action1.clampWhenFinished = true;
        action1.loop = THREE.LoopOnce; 
      }
    }
  }, [keycapList, count, pressedKey]);

  useFrame((state, delta) => {
    mixer?.update(delta);
  });


  return (
    <group onUpdate={(scene) => setKeyboardModel(scene)} ref={ref}>
      <Box model={boxModel} setBoxScene={setBoxScene} /> 
      <group
        dispose={null}
      >
        {keycapList.map((model: any) => {
          const x = model.scene.children[0].position.x;
          const y = model.scene.children[0].position.y;
          const z = model.scene.children[0].position.z; 
          return ( 
            <>
              <mesh
                animations={model.animations}
                userData={{
                  name: model.scene.children[0].name,
                  uuid: model.uuid,
                }}
                name={model.scene.children[0].name}
                rotation={model.scene.children[0].rotation}
                position={model.scene.children[0].position}
                geometry={model.scene.children[0].geometry}
                material={model.scene.children[0].material}
              />
              <group
                name={switchModel?.children[0].name}
                rotation={switchModel?.children[0].rotation}
                position={[x, y - 0.01, z]}
              >
                {switchModel?.children[0]?.children.map((switchModelChild: any) => {
                  return(
                    <mesh
                      name={switchModelChild?.name}
                      rotation={switchModelChild?.rotation}
                      position={switchModelChild?.position}
                      geometry={switchModelChild?.geometry}
                      material={switchModelChild?.material}
                  />)
                })}
              </group>
            </>
          );
        })}
      </group>
    </group>
  );
};

const ConstructorKeyboard = () => {
  const { title, kitID, boxID, switchTypeID, boxTypeId } = useAppSelector(
    (state) => state.keyboardReduer
  );
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const [modelKit, setModelKit] = useState<{scene : THREE.Group<THREE.Object3DEventMap>, uuid: string | undefined, animations: THREE.AnimationClip[]}[]>([]);
  const [boxModel, setBoxModel] = useState<THREE.Group<THREE.Object3DEventMap>>();
  const [switchModel, setSwitchModel] = useState<THREE.Group<THREE.Object3DEventMap>>();
  const [keyboardModel, setKeyboardModel] = useState<THREE.Group<THREE.Object3DEventMap>>();

  const [getBoxesModel] = useLazyGetBoxesQuery();
  const [getSwitchModel] = useLazyGetSwitchQuery();
  const [getListKeyCaps, {data : keyCapsList}] = useLazyGetKeycapsQuery();
  const [getKeycapModel] = useLazyGetKeycapQuery();
  const [getSwitchSound,  {data: switchSound}] = useLazyGetSwitchSoundQuery();
  const [postKeyboard] = usePostKeyboardMutation();

  const orbitref = useRef(null);
  const ref = useRef(null);
  const refModel = useRef(null);
  const loader = new GLTFLoader();
  const exporter = new GLTFExporter();
  
  
  const saveKeyboard = () => {
    if (!orbitref.current && orbitref.current === null) return;
    //@ts-ignore
    orbitref.current.reset();
    const animations : THREE.AnimationClip[] = modelKit?.map((clip) => clip.animations[0]);

    setTimeout(() => {
      const options = {
        animations: animations,
      };
      let previewFile: string | undefined = undefined;
      //@ts-ignore
      ref?.current?.toBlob(
        (blob: any) => {
          previewFile = blob;
          if (!previewFile || !kitID || !boxID || !switchTypeID || !keyboardModel)
            return;
          exporter.parse(
            keyboardModel,
            (gltf) => {
              const jsonString = JSON.stringify(gltf);
              const blob = new Blob([jsonString], { type: "application/json" });
              const file = new File([blob], title + ".glb", {
                type: "application/json",
                lastModified: Date.now(),
              });
              postKeyboard({
                file: file,
                preview: previewFile,
                title: title,
                switchTypeID: switchTypeID,
                boxTypeID: boxTypeId,
              })
              .unwrap()
              .then(() => {
                dispatch(resetKeyBoardState())
                navigate('/')
                window.location.reload();
              })
            },
            (error) => console.log(error), options);
        },
        "image/webp",
        1
      );
    }, 500);
  };

  useEffect(() => {
    if (!boxID || !kitID || !switchTypeID || !title) return;
    getBoxesModel(boxID) 
      .unwrap()
      .then((payload) => {
        loader.parse(payload, "", (gltf) => {
          setBoxModel(gltf.scene);
        });
      });

    getSwitchModel(switchTypeID) 
      .unwrap()
      .then((payload) => {
        loader.parse(payload, "", (gltf) => {
          setSwitchModel(gltf.scene);
        });
      });

    getListKeyCaps({
      page: 1,
      pageSize: 200,
      kitID: kitID
    })

    getSwitchSound(switchTypeID);
  }, []);

  useEffect(() => {
    if (!keyCapsList) return;
    keyCapsList.map((keycap) => {
        if (!keycap?.id) return;
        setModelKit([])
        getKeycapModel(keycap?.id)
        .unwrap()
        .then((payload) => {
          loader.parse(payload, "", (gltf) => {
          setModelKit(prevModelKit => [...prevModelKit, {scene: gltf.scene, uuid: keycap?.id, animations: gltf.animations}]);
          });
        });
      })
  }, [keyCapsList])

  return (
    <>
      <Header />
      <div onClick={() => document.body.focus()}>
      <Grid sx={{ bgcolor: "#2D393B" }} container spacing={0}>
        <Grid
          sx={{ width: "100vw", height: "100vh", flexGrow: 1 }}
          item
          xs={10}
        >
          <Canvas gl={{ preserveDrawingBuffer: true }} ref={ref}>
          <PerspectiveCamera     
              makeDefault
              zoom={16}
              fov={90}
              position={[-10, 20, 20]}
            />
            <directionalLight  args={[0xffffff]} position={[0, 0, 3]} intensity={1} />
            <directionalLight  args={[0xffffff]} position={[0, 0, -3]} intensity={1} />
            <directionalLight  args={[0xffffff]} position={[0, -3, 0]} intensity={1} />
            <directionalLight  args={[0xffffff]} position={[0, 3, 0]} intensity={1} />
            <directionalLight  args={[0xffffff]} position={[-3, 0, 0]} intensity={1} />
            <directionalLight  args={[0xffffff]} position={[3, 0, 0]} intensity={1} />
            <directionalLight  args={[0xffffff]} position={[0, 3, 3]} intensity={1} />
            <directionalLight  args={[0xffffff]} position={[0, -3, -3]} intensity={1} />
            <directionalLight  args={[0xffffff]} position={[3, -3, 0]} intensity={1} />
            <directionalLight  args={[0xffffff]} position={[3, 3, 0]} intensity={1} />
            <directionalLight  args={[0xffffff]} position={[-3, 0, 3]} intensity={1} />
            <directionalLight  args={[0xffffff]} position={[3, 0, 3]} intensity={1} />
            <OrbitControls
              ref={orbitref}
              maxDistance={2}
              minDistance={1}
              enablePan={false}
              target={[0, 0, 0]}
            />
            {modelKit && 
              <mesh ref={refModel}>
                <Keyboard
                  keycapList={modelKit}
                  boxModel={boxModel}
                  switchModel={switchModel}
                  switchSound={switchSound}
                  setKeyboardModel={setKeyboardModel} 
                />
              </mesh> }
          </Canvas>
        </Grid>
        
        <Grid item xs={2}>
            <KeayboardComponents onSave={saveKeyboard} kitID={kitID} boxID={boxID} switchTypeID={switchTypeID}/>
        </Grid>
      </Grid>
      </div>
    </>
  );
};

export default ConstructorKeyboard;
