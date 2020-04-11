(function() {
  const API = "http://localhost:7071/api";
  const KEY = "";

  // axios defaults
  axios.defaults.headers.common["x-functions-key"] = KEY;

  new Vue({
    el: "#app",
    data: {
      showModal: false,
      products: [],
      brands: [],
      newProduct: { name: "", price: null, stockUnits: null, brand: {} },
      toast: {
        type: "danger",
        message: null,
        show: false
      }
    },
    mounted() {
      this.getProducts();
    },
    methods: {
      getProducts() {
        this.products = axios
          .get(`${API}/GetUsers`)
          .then(response => {
            this.products = response.data;
            // como sabemos que nos llegan ordenados ..
            this.products[0].logo = "img/cup.png";
            this.products[1].logo = "img/second.png";
            this.products[2].logo = "img/third.png";
          })
          .catch(err => {
            this.showError("Get", err.message);
          });
      },
      showError(action, message) {
        this.showToast(`${action} failed: ${message}`, "danger");
      },
      showSuccess(message) {
        this.showToast(message, "success");
      },
      showToast(message, type) {
        this.toast.message = message;
        this.toast.show = true;
        this.toast.type = type;
        setTimeout(() => {
          this.toast.show = false;
        }, 3000);
      }
    }
  });
})();
