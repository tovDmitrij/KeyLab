import { FC, useEffect, useRef, useState } from "react";
import { Canvas, useFrame, useLoader, useThree } from "@react-three/fiber";

import classes from "./ConstructorsSwitches.module.scss";
import * as THREE from "three";
import Header from "../../components/Header/Header";
import { Grid } from "@mui/material";
import {
  useGetSwitchQuery,
  useGetSwitchesQuery,
  useLazyGetSwitchQuery,
  useLazyGetSwitchesQuery,
} from "../../services/switchesService";
import SwitchesList from "../../components/List/SwitchesList/SwitchesList";
import { GLTFLoader } from "three/examples/jsm/Addons.js";
import { OrbitControls, PerspectiveCamera, useGLTF } from "@react-three/drei";

import { GLTF } from 'three-stdlib'
import BlueSwitch from "/src/assets/mxblue.glb?url"


const Switch: FC<any> = ({model}) =>  {
  return (
    <group rotation={[Math.PI / 2, 0, 0]} dispose={null}>
        <mesh geometry={model.children[0].children[0].geometry} material={model.children[0].children[0].material}/>
        <mesh geometry={model.children[0].children[1].geometry} material={model.children[0].children[1].material}/>
        <mesh geometry={model.children[0].children[2].geometry} material={model.children[0].children[2].material}/>
    </group>
  )
}

const ConstructorsSwitches = () => {
  const ref = useRef(null);
  
  const [getSwitch] = useLazyGetSwitchQuery();
  const [model, setModel] = useState<THREE.Group<THREE.Object3DEventMap>>();

  const { data } = useGetSwitchesQuery({
    page: 1,
    pageSize: 10,
  });

  const getId = async (id: string) => {
    await getSwitch(id)
      .unwrap()
      .then((payload) => {
        const loader = new GLTFLoader();
        loader.parse(payload, "", (gltf) => {
          setModel(gltf.scene);
        });
      });
  };

  return (
    <>
      <Header />
      <Grid sx={{ bgcolor: "#2D393B" }} container spacing={0}>
        <Grid
          sx={{ width: "100vw", height: "100vh", flexGrow: 1 }}
          className={classes.editor}
          item
          xs={10}
        >
          <Canvas  gl={{ preserveDrawingBuffer: true }} ref={ref}>
          <PerspectiveCamera     
              makeDefault
              zoom={60}
              fov={90}
              position={[-10, 10, 20]}
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
              maxDistance={2}
              minDistance={1}
              enablePan={false}
              target={[0, 0, 0]}
            />
            {model && ( <Switch model = {model}/>)}
          </Canvas>
        </Grid>
        <Grid item xs={2}>
          {data && <SwitchesList switches={data} handleChoose={getId} />}
        </Grid>
      </Grid>
    </>
  );
};

export default ConstructorsSwitches;
