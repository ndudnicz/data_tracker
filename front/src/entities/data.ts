export class Data {
    id!: number;
    value!: number;
    createdAt!: Date;

    constructor(id: number, value: number, createdAt: Date) {
        this.id = id;
        this.value = value;
        this.createdAt = createdAt;
    }
}