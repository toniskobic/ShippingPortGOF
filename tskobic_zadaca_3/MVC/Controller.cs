namespace tskobic_zadaca_3.MVC
{
    public class Controller
    {
        private Model? Model { get; set; }

        private View? View { get; set; }

        public void AddModel(Model model)
        {
            Model = model;
        }

        public void AddView(View view)
        {
            View = view;
        }

        public void SetModelState(string state)
        {
            Model?.SetState(state);
        }
    }
}
