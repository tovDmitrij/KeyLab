import { useEffect, useRef, useState } from "react";
import { Canvas, useFrame, useLoader, useThree } from "@react-three/fiber";

import * as THREE from "three";
import Header from "../../components/Header/Header";
import { Grid } from "@mui/material";
import { OrbitControls } from "@react-three/drei";
import KitsList from "../../components/List/KitsList/KitsList";
import { useGetDefaultKitsQuery } from "../../services/kitsService";
import { useLazyGetKeycapQuery, useLazyGetKeycapsQuery } from "../../services/keycapsService";
import KeycapsList from "../../components/List/KeycapsList/KeycapsList";
import { GLTFLoader } from "three/examples/jsm/Addons.js";
import { saveAs } from 'file-saver';

type TKeycaps = {
  /**
   * id бокса
   */
  id?: string;
  /**
   * название бокса
   */
  title?: string;

  /**
   * дата создания бокса
   */
  creationDate?: string;
};


const ConstructorKeys = () => {
  const ref = useRef(null);
  const refModel = useRef(null);
  const [idKit, setIdKit] = useState<string>();
  const [keycaps, setKeycaps] = useState<TKeycaps[]>();
  const [model, setModel] = useState<THREE.Group<THREE.Object3DEventMap>>();
  const [modelKit, setModelKit] = useState<THREE.Group<THREE.Object3DEventMap>[]>([]);
  const [color, setColor] = useState<any>(undefined);
  const [getKeycaps] = useLazyGetKeycapsQuery();  
  const [getKeycap] = useLazyGetKeycapQuery();
  const loader = new GLTFLoader();

  const { data } = useGetDefaultKitsQuery({
    page: 1,
    pageSize: 10,
  });

  const handleChoose = (id: string) => {
    setIdKit(id)
  }

  const handleChooseColor = (color: any) => { 
    setColor(color);
  }

  const handleChooseKey = (id: string) => {
    getKeycap(id)
    .unwrap()
    .then((payload) => {
      const loader = new GLTFLoader();
      loader.parse(payload, "", (gltf) => {
        setModel(gltf.scene);
      });
    });
  }

  useEffect(() => {
    if (!idKit) return;
    getKeycaps({
      page: 1,
      pageSize: 9999,
      kitID: idKit,
    })
      .unwrap()
      .then((data) => {
        setKeycaps(data);
      });
  }, [idKit]);

  useEffect(() => {
    if (!keycaps) return;
    // keycaps.map((keycap) => {
    //   if (!keycap?.id) return;
    //   setModelKit([])
    //   getKeycap(keycap?.id)
    //   .unwrap()
    //   .then((payload) => {
    //     loader.parse(payload, "", (gltf) => {
    //     setModelKit(prevModelKit => [...prevModelKit, gltf.scene]);
    //     });
    //   });
    // })
    let i = 0;
    while (i < 100) {
      getKeycap('2fefaf65-6e3e-41f8-91ff-7a46529f734a')
      .unwrap()
      .then((payload) => {
        loader.parse(payload, "", (gltf) => {
        setModelKit(prevModelKit => [...prevModelKit, gltf.scene]);
        });
      });
    
      i+=1; 
    }
  }, [keycaps]);

  console.log(modelKit);

  // useEffect(() => {
  //   console.log(model?.children[0].position.set(0, 0, 0));  
  //   //console.log(model?.children[0].position);  
  // }, [model]);

  useEffect(() => {
    model?.children[0].children[0].material?.color?.setRGB(color.r / 255, color.g / 255, color.b /  255);  
  }, [color])
 
  return (
    <>
      <Header />
      <Grid sx={{ bgcolor: "#2D393B" }} container spacing={2}>
        <Grid
          sx={{ width: "100vw", height: "100vh", flexGrow: 1 }}
          //className={classes.editor}
          item
          xs={10}
        >
          <Canvas ref={ref}>
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
            {modelKit && (
              <mesh ref={refModel}>
                {modelKit.map((item) => (
                  <primitive object={item} scale={12} />
                ))}
              </mesh>
            )}
          </Canvas>
        </Grid>
        <Grid item xs={2}>
          {/* {keycaps && <KeycapsList keykaps={keycaps} handleChooseKey={handleChooseKey} handleChooseColor={handleChooseColor}/>} */}
          {    <KitsList kits={data} handleChoose={handleChoose} /> }
        </Grid>
      </Grid>
    </>
  );
};

export default ConstructorKeys;
