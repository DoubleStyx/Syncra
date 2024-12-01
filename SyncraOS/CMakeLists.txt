cmake_minimum_required(VERSION 3.30)
project(SyncraEngine C)

set(CMAKE_C_STANDARD 23)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

add_executable(SyncraEngine src/main.c)

set(CGLM_DIR "${CMAKE_SOURCE_DIR}/external/cglm-master")
set(GLFW_DIR "${CMAKE_SOURCE_DIR}/external/glfw-master")
set(OPENXR_DIR "${CMAKE_SOURCE_DIR}/external/OpenXR-SDK-main")
set(VULKAN_HEADERS_DIR "${CMAKE_SOURCE_DIR}/external/Vulkan-Headers-main")

include_directories(
        ${CGLM_DIR}/include
        ${GLFW_DIR}/include
        ${OPENXR_DIR}/include
        ${VULKAN_HEADERS_DIR}/include
)

find_package(Vulkan REQUIRED)

target_link_libraries(${PROJECT_NAME}
        glfw
        Vulkan::Vulkan
        ${CGLM_DIR}/lib/libcglm.a
        ${OPENXR_DIR}/lib/openxr_loader.a
)