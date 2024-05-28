import { FC, useEffect, useRef, useState } from "react";
import { Canvas, ThreeEvent, useFrame, useLoader, useThree } from "@react-three/fiber";

import * as THREE from "three";
import Header from "../../components/Header/Header";
import { Container, Grid, Modal, TextField, Typography } from "@mui/material";
import { OrbitControls, PerspectiveCamera } from "@react-three/drei";
import KitsList from "../../components/List/KitsList/KitsList";
import { useGetDefaultKitsQuery, usePatchKitsPreviewMutation, usePostKitsMutation } from "../../services/kitsService";
import { useLazyGetKeycapQuery, useLazyGetKeycapsQuery, usePutKeycapMutation } from "../../services/keycapsService";
import KeycapsList from "../../components/List/KeycapSettings/KeycapSettings";
import { GLTFExporter, GLTFLoader } from "three/examples/jsm/Addons.js";
import { saveAs } from 'file-saver';
import KeycapSettings from "../../components/List/KeycapSettings/KeycapSettings";
import ModalCreateKits from "../../components/Modals/ModalCreateKits";

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

type props = {
  handleClick: (data: any) => void;
  setKitsScene: (scene: any) => void;
  models: any
}

const Kits: FC<props> = ({ models, handleClick, setKitsScene}) => {
  const [state, setState] = useState(false);

  return (
    <group 
      onUpdate={(scene) => setKitsScene(scene)} 
      onPointerUp={() => {setState(false)}}  
      onPointerDown={(e) => {e.button === 0 && (setState(true), handleClick(e))}} 
      onPointerMove={(e) => state && handleClick(e)} 
      dispose={null}
    > 
      {models.map((model: any) => {
        console.log(model)
        return (
          <mesh
            scale={model.scene.children[0].scale}
            animations={model.animations}
            userData={{name: model.scene.children[0].name, uuid: model.uuid}}
            name={model.scene.children[0].name}
            rotation={model.scene.children[0].rotation} 
            position={model.scene.children[0].position}
            geometry={model.scene.children[0].geometry}
            material={model.scene.children[0].material}   
          />
        );
      })}
   </group>
  );
};

const ConstructorKeys = () => {
  const ref = useRef(null);
  const orbitref = useRef(null);
  const refModel = useRef(null);
  const [idKit, setIdKit] = useState<string>();
  const [keycaps, setKeycaps] = useState<TKeycaps[]>();
  const [newIdKit, setNewIdKit] = useState<string>();
  const [kitsScene, setKitsScene] = useState<any>();
  const [idBoxType, setIdBoxType] = useState<string>();
  const [modal, setModal] = useState<boolean>(false);
  const [modelKit, setModelKit] = useState<{scene : THREE.Group<THREE.Object3DEventMap>, uuid: string | undefined, animations: THREE.AnimationClip[]}[]>([]);
  const [color, setColor] = useState<any>(undefined);
  const [title, setTitle] = useState<string>();
  const [postKits] = usePostKitsMutation();
  const [getKeycaps] = useLazyGetKeycapsQuery();  
  const [getKeycap] = useLazyGetKeycapQuery();
  const [putKeycaps] = usePutKeycapMutation();
  const [patchKitsPreview] = usePatchKitsPreviewMutation();

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

  const handleNew = (boxTypeId: string) => {
    setIdBoxType(boxTypeId);
    setModal(true)
  }

  const onSubmitTitleModal = (title : string) => {
    postKits({
      title: title,
      boxTypeID: idBoxType,
    }).unwrap().then((data) => {
      setNewIdKit(data.kitID); 
      setTitle(title);  
      setModal(false);
    });
  }

  const saveKit = () => {
    if (!orbitref.current) return;
    //@ts-ignore
    kitsScene.children.map((keycap: any) => {
      saveKeycap(keycap);
    })
    //@ts-ignore
    orbitref.current.reset();
    setTimeout(() => {
      let previewFile: string | undefined = undefined;
      //@ts-ignore
      ref?.current?.toBlob((blob: any) => {
      previewFile = blob;
      patchKitsPreview({
          kitID: newIdKit,
          preview: previewFile
        })
          .unwrap()
          .then(() => setNewIdKit(undefined));
      }, "image/webp", 1);
    }, 500);
  }

  const handleClick = (e:  ThreeEvent<MouseEvent>) => {
    if (e.object.parent === null) return;
    //@ts-ignore
    e.object.material?.color?.setRGB(color.r  / 255, color.g / 255, color.b / 255);  
    e.stopPropagation(); 
  }

  const saveKeycap = (model : THREE.Object3D<THREE.Object3DEventMap>) => {
    const exporter = new GLTFExporter();
    if (!model) return; 
    const options = {
      animations: model.animations,
    };
    exporter.parse(model, (gltf) => { 
      const jsonString = JSON.stringify(gltf);
      const blob = new Blob([jsonString], { type: "application/json" });
      const file = new File([blob], model.name + ".glb", {  type: "application/json", lastModified: Date.now() });
      putKeycaps({
        file: file,
        keycapID: model.userData.uuid,
      }).unwrap();
    }, (error) => console.log(error), options);
  }

  
  const paintFull = () => {
    if (kitsScene === null && !kitsScene) return;
    //@ts-ignore
    kitsScene.children.map((keycap: any) => {
      //@ts-ignore
      keycap.material?.color?.setRGB(color.r  / 255, color.g / 255, color.b / 255); 
      saveKeycap(keycap);
    })
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
    if (!newIdKit) return;
    getKeycaps({
      page: 1,
      pageSize: 9999,
      kitID: newIdKit,
    })
      .unwrap()
      .then((data) => {
        setKeycaps(data);
      });
  }, [newIdKit]);

  useEffect(() => {
    if (!keycaps) return;
    keycaps.map((keycap) => {
      if (!keycap?.id) return;
      setModelKit([])
      getKeycap(keycap?.id)
      .unwrap()
      .then((payload) => {
        loader.parse(payload, "", (gltf) => {
        setModelKit(prevModelKit => [...prevModelKit, {scene: gltf.scene, 
          uuid: keycap?.id, 
          animations: gltf.animations}]);
        });
      });
    })
  }, [keycaps]);

  const mouseButtons = {
    LEFT: 2,
    MIDDLE: THREE.MOUSE.ROTATE,
    RIGHT: THREE.MOUSE.PAN
  };
 
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
              position={[0, 20, 0]}
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
              mouseButtons = {mouseButtons}
              maxDistance={2}
              minDistance={1}
              enablePan={false}
              target={[0, 0, 0]}
            />
            {modelKit && 
              <mesh ref={refModel}>
                <Kits
                  models={modelKit}
                  handleClick={handleClick}
                  setKitsScene={setKitsScene}
                />
              </mesh> }
          </Canvas>
        </Grid>
        <Grid item xs={2}>
          {newIdKit && <KeycapSettings title={title} handleChooseColor={handleChooseColor} saveKit={saveKit} paintFull={paintFull}/>}
          {!newIdKit && <KitsList kits={data} handleChoose={handleChoose} handleNew={handleNew} /> }
        </Grid>
      </Grid>

      {modal && <ModalCreateKits open={modal} handleCloseModal={() => setModal(false)} onSubmitTitleModal={onSubmitTitleModal}/>}
    </>
  );
};

export default ConstructorKeys;
