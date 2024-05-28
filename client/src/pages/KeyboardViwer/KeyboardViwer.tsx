import { FC, useEffect, useRef, useState } from "react";
import { Canvas, useFrame, useLoader, useThree } from "@react-three/fiber";

import * as THREE from "three";
import Header from "../../components/Header/Header";
import { OrbitControls, PerspectiveCamera, useAnimations } from "@react-three/drei";
import { useLazyGetKeyBoardQuery } from "../../services/keyboardService";
import { GLTFLoader } from "three/examples/jsm/Addons.js";
import { useParams } from "react-router-dom";
import { Grid } from "@mui/material";


const KeyboardViwer  = () => {
  const [getKeyBoard] = useLazyGetKeyBoardQuery();
  const [model, setModel] = useState<THREE.Group<THREE.Object3DEventMap>>();  
  const { uuid } = useParams();
  const orbitref = useRef(null);
  const refModel = useRef(null);
  
  

  const getModel = (id: string) => {
    getKeyBoard(id)
      .unwrap()
      .then((payload) => {  
        const loader = new GLTFLoader();
        loader.parse(payload, "", (gltf) => {
          console.log(gltf)
          setModel(gltf.scene);
        });
      });
  };

  useEffect(() => {
    if (!uuid) return;
    getModel(uuid);
  }, [uuid]);

  return (
    <>
      <Header />
      <div onClick={() => document.body.focus()}>
      <Grid sx={{ bgcolor: "#2D393B" }} container spacing={0}>
        <Grid 
          sx={{ width: "100vw", height: "100vh", flexGrow: 1 }}
          item
        >
          <Canvas gl={{ preserveDrawingBuffer: true }}>
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
            {model &&
              <mesh ref={refModel}>
                <primitive
                  object={model}
                />
              </mesh> }
          </Canvas>
          </Grid>
      </Grid>
      </div> 
    </>
  );
};

export default KeyboardViwer;
