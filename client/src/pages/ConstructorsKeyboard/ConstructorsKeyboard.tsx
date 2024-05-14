import { FC, useEffect, useRef, useState } from "react";
import { Canvas, useFrame, useLoader, useThree } from "@react-three/fiber";

import * as THREE from "three";
import Header from "../../components/Header/Header";
import { Grid } from "@mui/material";
import { GLTFExporter, GLTFLoader } from "three/examples/jsm/Addons.js";
import { OrbitControls, PerspectiveCamera, useAnimations } from "@react-three/drei";
import BoxesList from "../../components/List/ListBoxes/BoxesList";
import { useLazyGetBoxesQuery, usePostBoxMutation } from "../../services/boxesService";
import ListBoxesNew from "../../components/List/ListBoxes/ListBoxesNew"
import { useAppSelector } from "../../store/redux";
import { useLazyGetSwitchQuery } from "../../services/switchesService";
import { useLazyGetKeycapQuery, useLazyGetKeycapsQuery } from "../../services/keycapsService";
import Box from "../../components/Models/Box";
import KeayboardComponents from "../../components/KeayboardComponents/KeayboardComponents";

const Keyboard: FC<any> = ({ keycapList,boxModel, switchModel}) => {
  const [boxScene, setBoxScene] = useState<THREE.Group<THREE.Object3DEventMap>>();
  const ref = useRef(null)

  let mixer: THREE.AnimationMixer;
  if (keycapList?.[1]?.animations.length && ref && ref.current !== null) {
      mixer = new THREE.AnimationMixer(ref.current);

      const action = mixer.clipAction(keycapList?.[1]?.animations?.[0])
      action.play();

  }

  useFrame((state, delta) => {
      mixer?.update(delta)
  })

  return (

    <group ref={ref}>
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
  const { title, kitID, boxID, switchTypeID } = useAppSelector(
    (state) => state.keyboardReduer
  );

  const [modelKit, setModelKit] = useState<{scene : THREE.Group<THREE.Object3DEventMap>, uuid: string | undefined, animations: THREE.AnimationClip[]}[]>([]);
  const [boxModel, setBoxModel] = useState<THREE.Group<THREE.Object3DEventMap>>();
  const [switchModel, setSwitchModel] = useState<THREE.Group<THREE.Object3DEventMap>>();
  const [getBoxesModel] = useLazyGetBoxesQuery();
  const [getSwitchModel] = useLazyGetSwitchQuery();
  const [getListKeyCaps, {data : keyCapsList}] = useLazyGetKeycapsQuery();
  const [getKeycapModel] = useLazyGetKeycapQuery();
  const refModel = useRef(null);
  const loader = new GLTFLoader();
  
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
  },[])

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


  // if (modelKit && modelKit[0] && modelKit[0]?.animations &&  modelKit[0]?.scene) {
  //   const { actions, mixer } = useAnimations(modelKit[0]?.animations, modelKit[0]?.scene)
  //   console.log(actions)
  // }

  const orbitref = useRef(null);
  const ref = useRef(null);

  return (
    <>
      <Header />
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
              position={[0, 20, 20]}
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
                />
              </mesh> }
          </Canvas>
        </Grid>
        
        <Grid item xs={2}>
            <KeayboardComponents kitID={kitID} boxID={boxID} switchTypeID={switchTypeID}/>
        </Grid>
      </Grid>
    </>
  );
};

export default ConstructorKeyboard;
