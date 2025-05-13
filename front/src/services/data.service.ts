import type { Data } from "../entities/data";

export class DataService {
    async getAll(): Promise<Data[]>  {
        const response = await fetch('http://localhost:5000/v1/data');
        const data = await response.json();
        console.log('getAll', data);
        
        return data;
    }

    async getById(id: number): Promise<Data> {
        const response = await fetch(`http://localhost:5000/v1/data/${id}`);
        const data = await response.json();
        console.log('getById', data);
        
        return data;
    }
    
    async create(data: Data): Promise<Data> {
        const response = await fetch('http://localhost:5000/v1/data', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data),
        });
        const newData = await response.json();
        console.log('create', newData);
        
        return newData;
    }
}