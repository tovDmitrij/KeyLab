export class DownloadService {
    downloadImg() {
        const link = document.createElement('a');
        link.setAttribute('download', 'CanvasAsImage.png');

        const canvas = document.getElementsByTagName('canvas')[0];
        canvas.toBlob((blob) => {
            const url = URL.createObjectURL(blob);
            link.setAttribute('href', url);
            link.click();
        });
    }
}