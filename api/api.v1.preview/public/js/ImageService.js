export class ImageService {
    link

    constructor() {
        this.link = document.createElement('a')
        this.link.setAttribute('download', 'img.jpeg')
    }

    download(source) {
        source.toBlob((blob) => {
            const url = URL.createObjectURL(blob)
            this.link.setAttribute('href', url)
            this.link.click()
        })
    }

    get(source) {
        source.toBlob((blob) => {
            return blob
        })
    }
}