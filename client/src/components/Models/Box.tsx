import { FC } from "react";

const Box: FC<any> = ({ model, setBoxScene }) => {
  

  return (
    <group
      onUpdate={(scene) => setBoxScene(scene)}
      name={model?.name}
      rotation={model?.rotation}
      position={model?.position}
    >
      {model?.children?.map((model: any) => {
        if (
          model &&
          model?.visible === true &&
          !(model?.name?.includes("Switch") || model?.name?.includes("keycap"))
        ) {
          return model?.type?.includes("Mesh") ? (
            <mesh
              type={model?.type}
              scale={model?.scale}
              name={model?.name}
              rotation={model?.rotation}
              position={model?.position}
              geometry={model?.geometry}
              material={model?.material}
            />
          ) : (
            <group
              name={model?.name}
              rotation={model?.rotation}
              position={model?.position}
            >
              {model?.children?.map((modelChild: any) => {
                if (modelChild?.type?.includes("Mesh")) {
                  return (
                    <mesh
                      type={modelChild?.type}
                      scale={modelChild?.scale}
                      name={modelChild?.name}
                      rotation={modelChild?.rotation}
                      position={modelChild?.position}
                      geometry={modelChild?.geometry}
                      material={modelChild?.material}
                    />
                  )
                } else {
                  return (
                  <group 
                    name={modelChild?.name}
                    rotation={modelChild?.rotation}
                    position={modelChild?.position}
                  >
                    {modelChild.children?.map((modelChildChild: any) => {
                      return (
                        <mesh
                          type={modelChildChild?.type}
                          scale={modelChildChild?.scale}
                          name={modelChildChild?.name}
                          rotation={modelChildChild?.rotation}
                          position={modelChildChild?.position}
                          geometry={modelChildChild?.geometry}
                          material={modelChildChild?.material}
                        />
                      )
                    })}
                  </group>
                )};
              })}
            </group>
          )
        }
        return null;
      })}
    </group>
  );
};

export default Box;