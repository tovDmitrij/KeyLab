import * as THREE from 'https://unpkg.com/three@0.162.0/build/three.module.js';
import { OrbitControls } from 'https://unpkg.com/three@0.162.0/examples/jsm/controls/OrbitControls.js';
import { GLTFLoader } from 'https://unpkg.com/three@0.162.0/examples/jsm/loaders/GLTFLoader.js';



export class GLTFCanvas {
    scene;
    ambientLight;
    directionalLight;
    renderer;
    mesh;
    camera;
    controls;
    loader;
    animation = 0;
    currentModel;
    isCompleted = false;

    constructor(cameraPosition, background, width, height) {
        this.scene = new THREE.Scene();

        this.renderer = new THREE.WebGLRenderer({ 
            antialias: true, 
            alpha: true, 
            preserveDrawingBuffer: true
        });
        this.renderer.setSize(width, height);

        this.ambientLight = new THREE.AmbientLight(0xffffff, 1);
        this.scene.add(this.ambientLight);

        this.directionalLight = new THREE.DirectionalLight(0xffffff, 1);
        this.directionalLight.position.set(0, 0, 2);
        this.scene.add(this.directionalLight);

        const geometry = new THREE.SphereGeometry(10, 10, 10);
        geometry.scale(- 1, 1, 1);
        const material = new THREE.MeshBasicMaterial({
            map: new THREE.TextureLoader().load(background)
        })
        this.mesh = new THREE.Mesh(geometry, material);
        this.scene.add(this.mesh);

        this.camera = new THREE.PerspectiveCamera(75, 16 / 9, 0.01, 1000);
        this.camera.position.set(cameraPosition.x, cameraPosition.y, cameraPosition.z);

        this.controls = new OrbitControls(this.camera, this.renderer.domElement);
        this.controls.update();

        this.loader = new GLTFLoader();
    }

    getRendererDom() {
        return this.renderer.domElement;
    }

    loadModel(modelPosition, file) {
        if (this.animation !== 0) {
            cancelAnimationFrame(this.animation);
        }
        
        this.scene.remove(this.currentModel);
        this.isCompleted = false;

        this.loader.load(file, (gltf) => {
            const model = gltf.scene;
            model.position.setX(modelPosition.x);
            model.position.setY(modelPosition.y);
            model.position.setZ(modelPosition.z);
            model.rotation.x = 0.975;

            this.currentModel = gltf.scene;
            this.scene.add(gltf.scene);
            
            gltf.animations;
            gltf.scene;
            gltf.scenes;
            gltf.cameras;
            gltf.asset;
        }, (xhr) => {
            const stage = xhr.loaded / xhr.total * 100;
            if (stage === 100) {
                this.isCompleted = true;
            }
        });

        const animate = () => {
            this.animation = requestAnimationFrame(animate);
            this.controls.update();
            this.renderer.render(this.scene, this.camera);
        };
        animate();
    }
}