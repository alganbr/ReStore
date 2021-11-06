import axios, {AxiosError, AxiosResponse} from 'axios';
import { toast } from 'react-toastify';
import { Product } from '../models/product';
import { history } from '../..';

const sleep = (delay: number) => {
  return new Promise(resolve => {
    setTimeout(resolve, delay);
  })
}

axios.defaults.baseURL = 'https://localhost:5001/api';

axios.interceptors.response.use( async response => {
  await sleep(1000);
  return response;
}, (error: AxiosError) => {
  const {data, status} = error.response!;
  switch (status) {
    case 400:
      if (data.errors) {
        const modelStateErrors: string[] = [];
        for (const key in data.errors) {
          if (data.errors[key]) {
            modelStateErrors.push(data.errors[key]);
          }
        }
        throw modelStateErrors.flat();
      }
      toast.error(data.title);
      break;
    case 401:
      toast.error(data.title);
      break;
    case 500:
      history.push({
        pathname: '/server-error',
        state: {error: data}
      });
      break;
    default:
      break;
  }
  return Promise.reject(error.response);
})

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

const requests = {
  get: <T>(url: string) => axios.get<T>(url).then(responseBody),
  post: <T>(url: string, body: T) => axios.post<T>(url, body).then(responseBody),
  put: <T>(url: string, body: T) => axios.put<T>(url, body).then(responseBody),
  delete: <T>(url: string) => axios.delete<T>(url).then(responseBody),
}

const Catalog = {
  list: () => requests.get<Product[]>('/products'),
  details: (id: string) => requests.get<Product>(`/products/${id}`),
  create: (product: Product) => requests.post('/products', product),
  update: (product: Product) => requests.put(`/products/${product.id}`, product),
  delete: (id: string) => requests.delete(`/products/${id}`)
}

const TestErrors = {
  get400Error: () => requests.get('buggy/bad-request'),
  get401Error: () => requests.get('buggy/unauthorized'),
  get404Error: () => requests.get('buggy/not-found'),
  get500Error: () => requests.get('buggy/server-error'),
  getValidationError: () => requests.get('buggy/validation-error'),
}

const agent = {
  Catalog,
  TestErrors
}

export default agent;