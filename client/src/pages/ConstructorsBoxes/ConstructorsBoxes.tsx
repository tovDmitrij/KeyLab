import { FC, useEffect, useRef, useState } from "react";
import { Canvas, useFrame, useLoader, useThree } from "@react-three/fiber";

import * as THREE from "three";
import Header from "../../components/Header/Header";
import { Grid } from "@mui/material";
import { GLTFExporter, GLTFLoader } from "three/examples/jsm/Addons.js";
import { OrbitControls, PerspectiveCamera } from "@react-three/drei";
import BoxesList from "../../components/List/ListBoxes/BoxesList";
import {
  useLazyGetBoxesQuery,
  usePostBoxMutation,
} from "../../services/boxesService";
import ListBoxesNew from "../../components/List/ListBoxes/ListBoxesNew";
import Box from "../../components/Models/Box";
import { useAppDispatch } from "../../store/redux";
import { setBoxID, setBoxTitle, setBoxTypeId } from "../../store/keyboardSlice";

const ConstrucrotBoxes = () => {
  const [getBoxes] = useLazyGetBoxesQuery();
  const [postBox] = usePostBoxMutation();
  const [boxScene, setBoxScene] = useState<THREE.Group<THREE.Object3DEventMap>>();
  const [model, setModel] = useState<THREE.Group<THREE.Object3DEventMap>>();
  const [newIdBoxType, setNewIdBoxType] = useState<string | undefined>( undefined);
  const [color, setColor] = useState<any>(undefined);
  const dispatch = useAppDispatch();
  const orbitref = useRef(null);
  const ref = useRef(null);

  const getId = (id: string) => {
    getBoxes(id)
      .unwrap()
      .then((payload) => {
        const loader = new GLTFLoader();
        loader.parse(payload, "", (gltf) => {
          console.log(gltf)
          setModel(gltf.scene);
        });
      });
  };

  const saveNewBox = (title: string) => {
    const exporter = new GLTFExporter();
    if (!orbitref.current && orbitref.current === null) return;
    //@ts-ignore
    orbitref.current.reset();
    setTimeout(() => {
      let previewFile: string | undefined = undefined;
      //@ts-ignore
      ref?.current?.toBlob(
        (blob: any) => {
          previewFile = blob;
          if (!previewFile || !newIdBoxType || !boxScene)
            return;
          exporter.parse(
            boxScene,
            (gltf) => {
              const jsonString = JSON.stringify(gltf);
              const blob = new Blob([jsonString], { type: "application/json" });
              const file = new File([blob], title + ".glb", {
                type: "application/json",
                lastModified: Date.now(),
              });
              postBox({
                file: file,
                preview: previewFile,
                title: title,
                typeID: newIdBoxType,
              })
                .unwrap()
                .then((data) => {
                  dispatch(setBoxTitle(title));
                  dispatch(setBoxID(data.boxID));
                  dispatch(setBoxTypeId(newIdBoxType));
                  setNewIdBoxType(undefined);
                });
            },
            (error) => console.log(error)
          );
        },
        "image/webp",
        1
      );
    }, 500);
  };

  const newBox = (idType: string, idBaseBox: string) => {
    setNewIdBoxType(idType);
    getId(idBaseBox);
  };

  const handleChooseColor = (color: any) => {
    setColor(color);
  };
  
  useEffect(() => {
    if (!color) return;
    model?.children.forEach((child) => {
      if (
        !(
          child.name.includes("Cube") ||
          child.name.includes("Stabilize") ||
          child.name.includes("Plate")
        ) &&
        child.visible === true
      ) {
        //@ts-ignore
        child?.material?.color?.setRGB(
          color.r / 255,
          color.g / 255,
          color.b / 255
        );
      }
    });
  }, [color]);

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
            <directionalLight  args={[0xffffff]} position={[0, -3, -3]} intensity={0.1} />
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
            { model && <Box model={model} setBoxScene={setBoxScene}/> }
          </Canvas>
        </Grid>

        <Grid item xs={2}>
          {!newIdBoxType && (
            <BoxesList handleChoose={getId} handleNew={newBox} />
          )}
          {newIdBoxType && (
            <ListBoxesNew
              saveNewBox={saveNewBox}
              handleChooseColor={handleChooseColor}
            />
          )}
        </Grid>
      </Grid>
    </>
  );
};

export default ConstrucrotBoxes;