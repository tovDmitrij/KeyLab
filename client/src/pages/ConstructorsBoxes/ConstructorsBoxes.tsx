import { useEffect, useRef, useState } from "react";
import { Canvas, useFrame, useLoader, useThree } from "@react-three/fiber";

import classes from "./ConstructorsSwitches.module.scss";
import * as THREE from "three";
import Header from "../../components/Header/Header";
import { Grid } from "@mui/material";
import { GLTFExporter, GLTFLoader } from "three/examples/jsm/Addons.js";
import { saveAs } from "file-saver";
import { OrbitControls } from "@react-three/drei";
import BoxesList from "../../components/List/ListBoxes/ListBoxes";
import { useGetAuthBoxesQuery, useGetBoxesQuery, useGetDefaultBoxesQuery, useLazyGetBoxesQuery, useLazyGetDefaultBoxesQuery, usePostBoxMutation } from "../../services/boxesService";
import ListBoxesNew from "../../components/List/ListBoxes/ListBoxesNew";

export enum typeBoxesId { 
  boxes100 = 'f27d815d-8702-4853-9df8-482a95bd6aaa', 
  boxes75 = '809c62fe-8c6a-4ae4-b90d-9b112cbba86d',
  boxes60 = '63a9640a-8763-4101-8294-5b37e796bb9b', 
  boxes40 = '782f1e2b-5eaa-4452-ae82-0427fbecaefd'
};

const ConstrucrotBoxes = () => {
  const [getBoxes] = useLazyGetBoxesQuery();
  const [postBox] = usePostBoxMutation();

  const [model, setModel] = useState<THREE.Group<THREE.Object3DEventMap>>();
  const [newIdBoxType, setNewIdBoxType] = useState<string | undefined>(undefined);
  const [previewFile, setPreviewFile] = useState();
  const [color, setColor] = useState<any>(undefined);
  const ref = useRef(null);
  const refModel = useRef(null);

  const dataBase40Resp = {
    page: 1,
    pageSize: 10,
    typeID: typeBoxesId.boxes40
  }

  const dataBase60Resp = {
    page: 1,
    pageSize: 10,
    typeID: typeBoxesId.boxes60
  }
  
  const dataBase75Resp = {
    page: 1,
    pageSize: 10,
    typeID: typeBoxesId.boxes75
  }

  const dataBase100Resp = {
    page: 1,
    pageSize: 10,
    typeID: typeBoxesId.boxes100
  }
  
  const { data : dataBase40 } = useGetDefaultBoxesQuery(dataBase40Resp);
  const { data : dataBase60 } = useGetDefaultBoxesQuery(dataBase60Resp);
  const { data : dataBase75 } = useGetDefaultBoxesQuery(dataBase75Resp);
  const { data : dataBase100 } = useGetDefaultBoxesQuery(dataBase100Resp);
  const { data : data40 } = useGetAuthBoxesQuery(dataBase40Resp);
  const { data : data60 } = useGetAuthBoxesQuery(dataBase60Resp);
  const { data : data75 } = useGetAuthBoxesQuery(dataBase75Resp);
  const { data : data100 } = useGetAuthBoxesQuery(dataBase100Resp);

  const getId = (id: string) => {
     getBoxes(id)
      .unwrap()
      .then((payload) => {
        const loader = new GLTFLoader();
        loader.parse(payload, "", (gltf) => {
          setModel(gltf.scene);
        });
      });
  };

  const saveNewBox = (title : string) => {

    const exporter = new GLTFExporter();
   
    if (refModel.current === null || !previewFile || !newIdBoxType) return;
    exporter.parse(refModel?.current, (gltf) => {
      const jsonString = JSON.stringify(gltf);
      const blob = new Blob([jsonString], { type: "application/json" });
      const file = new File([blob], title + ".glb", {  type: "application/json", lastModified: Date.now() });
      postBox({
        file: file,
        preview: previewFile,
        title: title,
        typeID: newIdBoxType,
      }).unwrap().then(() => setNewIdBoxType(undefined));
    }, (error) => console.log(error));
  }

  const newBox = (idType: string) => {
    setNewIdBoxType(idType);
    const key = Object.keys(typeBoxesId).find(key => typeBoxesId[key as keyof typeof typeBoxesId] === idType) as keyof typeof typeBoxesId | undefined;

    //todo: поменять на boxes/types
    switch(key) { 
      case "boxes100": { 
        if (dataBase100 && dataBase100[0] && dataBase100[0].id) getId(dataBase100[0].id);
        break; 
      } 
      case "boxes75": { 
        if (dataBase75 && dataBase75[0] && dataBase75[0].id)  getId(dataBase75[0]?.id);
        break; 
      } 
      case "boxes60": {
        if (dataBase60 && dataBase60[0] && dataBase60[0].id)  getId(dataBase60[0]?.id);
        break;    
      } 
      case "boxes40": { 
        if (dataBase40 && dataBase40[0] && dataBase40[0].id)  getId(dataBase40[0]?.id);
        break; 
      }  
    }
  }

  const handleChooseColor = (color: any) => { 
    setColor(color);
  }

  useEffect(() => {
    model?.children.forEach( child => {
      if ((child.name.includes('Switch')) || (child.name.includes('keycap'))) {
        child.visible = false;
      }
    })
  }, [model])

  useEffect(() => {
    if (!color) return;
    model?.children.forEach( child => {
      if (!((child.name.includes('Cube'))  || (child.name.includes('Stabilize'))  || (child.name.includes('Plate'))) && child.visible === true) {
        //@ts-ignore
        child?.material?.color?.setRGB(color.r / 255, color.g / 255, color.b /  255)
      }
    })
  }, [color])

  useEffect(()=> {
    if (!ref.current && ref.current === null) return;
    //@ts-ignore
    ref?.current?.toBlob((blob: any) => {
      setPreviewFile(blob)
    }, 'image/png');
  }, [model, color])
 
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
          <Canvas camera={{ fov: 90, zoom: 10, position: [0, 10, 8]}} gl={{ preserveDrawingBuffer: true }} ref={ref}>
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
              maxDistance={2}
              minDistance={1}
              enablePan={false}
              target={[0, 0, 0]}
            />
            {model && (
              <mesh ref={refModel}>
                <primitive object={model} />
              </mesh>
            )}
          </Canvas>
        </Grid>
        <Grid item xs={2}>
          {!newIdBoxType && (
            <BoxesList
              boxes40={data40}
              boxes60={data60}
              boxes75={data75}
              boxes100={data100}
              handleChoose={getId}
              handleNew={newBox}
            />
          )}
          {newIdBoxType && (
            <ListBoxesNew saveNewBox={saveNewBox} handleChooseColor={handleChooseColor}/>
          )}
        </Grid>
      </Grid>
    </>
  );
};

export default ConstrucrotBoxes;
