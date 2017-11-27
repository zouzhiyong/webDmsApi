<template> 
<layout>
  <moduleTree slot="tree" @node-click="handleNodeClick"></moduleTree>   
  <moduleTable slot="table" ref="table" @edit-click="handleEditClick"></moduleTable> 
  <moduleForm slot="form" ref="form"></moduleForm>
</layout>
</template>

<script>
import layout from "./../layout/layout_left_right";
import moduleTree from "./sys_module_tree";
import moduleTable from "./sys_module_table";
import moduleForm from "./sys_module_form";

export default {
  components: {
    layout,
    moduleForm,
    moduleTree,
    moduleTable
  },
  methods: {
    handleNodeClick(data) {
      this.$refs.table.conditionData = data;
      this.$refs.table.GetData();
    },
    handleEditClick(row) {
      this.$refs.form.GetData(row);
    }
  }
};
</script>

