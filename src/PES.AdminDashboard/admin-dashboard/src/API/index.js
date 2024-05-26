const url = "http://localhost:5046/api/v1/";

export const getOrders = async () => {
    const res = await fetch("https://dummyjson.com/carts/1");
    return await res.json();
};

export const getRevenue = async () => {
    const res = await fetch("https://dummyjson.com/carts");
    return await res.json();
};

export const getProduct = async () => {
    const res = await fetch("http://localhost:5046/api/v1/Product?pageNumber=0&pageSize=10");
    return await res.json();
};
export const getProductDetail = async ({id}) => {
    const res = await fetch(`http://localhost:5046/api/v1/Product/${id}`);
    return await res.json();
};

export const getCustomers = async () => {
    const res = await fetch("https://dummyjson.com/users");
    return await res.json();
};
export const getComments = async () => {
    const res = await fetch("https://dummyjson.com/comments");
    return await res.json();
};

export const getCategories = async () => {
    const res = await fetch("http://localhost:5046/api/v1/Category");
    return await res.json();
};

export const getCategoryDetail = async ({ id }) => {
    const res = await fetch(`http://localhost:5046/api/v1/Category/${id}`);
    return await res.json();
  };

  export const getUsers = async () => {
    const res = await fetch("http://localhost:5046/api/v1/User");
    return await res.json();
};


