import { useEffect, useRef, useState } from "react";
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
import { saveAs } from "file-saver";
import { OrbitControls } from "@react-three/drei";

const ConstructorsSwitches = () => {
  const [getSwitch] = useLazyGetSwitchQuery();
  const [model, setModel] = useState<THREE.Group<THREE.Object3DEventMap>>();

  const loader = new GLTFLoader();

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
      <Grid sx={{ bgcolor: "#2D393B" }} container spacing={2}>
        <Grid
          sx={{ width: "100vw", height: "100vh", flexGrow: 1 }}
          className={classes.editor}
          item
          xs={10}
        >
          <Canvas>

            <directionalLight  args={[0xffffff]} position={[0, 0, 3]} intensity={1} />
            <directionalLight  args={[0xffffff]} position={[0, 0, -3]} intensity={1} />
            <directionalLight  args={[0xffffff]} position={[0, -3, 0]} intensity={1} />
            <directionalLight  args={[0xffffff]} position={[0, 3, 0]} intensity={1} />
            <directionalLight  args={[0xffffff]} position={[-3, 0, 0]} intensity={1} />
            <directionalLight  args={[0xffffff]} position={[3, 0, 0]} intensity={1} />

            <directionalLight  args={[0xffffff]} position={[0, 3, 3]} intensity={1} />
            <directionalLight  args={[0xffffff]} position={[0, -3, -3]} intensity={0.1} />
            <directionalLight  args={[0xffffff]} position={[3, -3, 0]} intensity={1} />
            <directionalLight  args={[0xffffff]} position={[3, 3, 0]} intensity={1} />
            <directionalLight  args={[0xffffff]} position={[-3, 0, 3]} intensity={1} />
            <directionalLight  args={[0xffffff]} position={[3, 0, 3]} intensity={1} />
            
            <OrbitControls
              maxPolarAngle={Math.PI / 2.2}
              minPolarAngle={Math.PI / 20}
              maxDistance={2}
              minDistance={1}
              enablePan={false}
              target={[0, 0, 0]}
            />
            {model && <primitive object={model} scale={"50"} />}
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
